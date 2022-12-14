using Chess.Logic;
using Chess.Logic.Dtos;
using Chess.Web.Data;
using Chess.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Chess.Web.Controllers
{
    [ApiController]
    [Authorize]
    public class ChessController : Controller
    {
        private readonly ILogger<ChessController> _logger;
        private readonly IHubContext<ChessHub, IChessClient> hubContext;
        private readonly ChessContext context = new();

        private static readonly Dictionary<ClockSettings, List<string>> waitingUsersMap = new();
        private static readonly List<Match> matches = new();

        private string CurrentUser => User!.Identity!.Name!;

        public ChessController(ILogger<ChessController> logger, IHubContext<ChessHub, IChessClient> hubContext)
        {
            _logger = logger;
            this.hubContext = hubContext;
        }

        [HttpGet]
        [Route("~/chess/getsearchinggame")]
        public string GetSearchingGameJson() =>
            GetSearchingGame().ToJson();
            
        private ClockSettings GetSearchingGame() =>
            waitingUsersMap.FirstOrDefault(x => x.Value.Contains(CurrentUser)).Key;

        [HttpPost]
        [Route("~/chess/searchgame")]
        public string SearchGame([FromBody] ClockSettings settings)
        {
            if(matches.Any(x => x.ContainsUser(CurrentUser)))
                return "Player's playing the game";

            var searchingGame = GetSearchingGame();
            if (searchingGame != default)
                waitingUsersMap[searchingGame].Remove(CurrentUser);

            if (!waitingUsersMap.ContainsKey(settings))
                waitingUsersMap[settings] = new();

            var users = waitingUsersMap[settings];
            users.Add(CurrentUser);

            if (users.Count == 2)
            {
                var game = new Game(settings.Time, settings.Bonus);
                var match = new Match(users[0], users[1], game);
                game.OnGameEnd += () => SendGameState(match);
                matches.Add(match);

                foreach (var player in match.Players)
                {
                    var userId = GetUserId(player.Username);
                    hubContext.Clients.User(userId)
                        .StartGame();
                }
                waitingUsersMap.Remove(settings);
            }

            return "true";
        }

        [HttpPost]
        [Route("~/chess/stopsearch")]
        public bool StopSearch()
        {
            var settings = GetSearchingGame();

            if (settings != default)
                waitingUsersMap[settings].Remove(CurrentUser);

            return true;
        }

        [HttpPost]
        [Route("~/chess/offerdraw")]
        public bool OfferDraw()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("~/chess/closegame")]
        public void CloseGame()
        {
            var match = GetMatch();
            if (match is null)
                return;

            var player = match.Players.First(x => x.Username == CurrentUser);

            match.Players.Remove(player);

            if (match.Players.Count == 0)
                matches.Remove(match);
        }

        [HttpPost]
        [Route("~/chess/resign")]
        public bool Resign()
        {
            var match = GetMatch();
            if (match is null)
                return false;

            match.Game.Resign(match.GetColor(CurrentUser));
            return true;
        }

        [HttpGet]
        [Route("~/chess/gettime")]
        public string GetTime([FromQuery] PlayerColor player)
        {
            var match = GetMatch();
            if (match is null)
                return new TimerDto(-1, false).ToJson();

            return match.Game.TimersMap[player].ToJson();
        }

        [HttpGet]
        [Route("~/chess/getgamestate")]
        public string GetGameState()
        {
            var match = GetMatch();
            if (match is null)
                return new object().ToJson();

            var gameState = new GameState(match.Game, match.GetColor(CurrentUser));
            return gameState.ToJson();
        }

        [HttpPost]
        [Route("~/chess/makemove")]
        public bool MakeMove([FromBody] JsonElement data)
        {
            var moveCode = data.Deserialize<string>();

            var match = GetMatch();

            if (match is null)
                return false;

            var isValidMove = match.Game.TryMakeMove(moveCode);

            if (!isValidMove)
                return false;

            SendGameState(match);

            return true;
        }

        private Match? GetMatch() =>
            matches.FirstOrDefault(x => x.ContainsUser(CurrentUser));

        private void SendGameState(Match match)
        {
            foreach (var player in match.Players)
            {
                var userId = GetUserId(player.Username);
                var json = new GameState(match.Game, player.Color).ToJson();
                hubContext.Clients.User(userId).UpdateBoard(json);
            }
        }

        private string GetUserId(string username) =>
            context.Users.First(x => x.UserName == username).Id;
    }
}
