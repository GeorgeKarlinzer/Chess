using Chess.Logic;
using Chess.Web.Data;
using Chess.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        private static readonly List<string> usersQueue = new();
        private static readonly List<Match> matches = new();

        private string CurrentUser => User!.Identity!.Name!;

        public ChessController(ILogger<ChessController> logger, IHubContext<ChessHub, IChessClient> hubContext)
        {
            _logger = logger;
            this.hubContext = hubContext;
        }

        [HttpGet]
        [Route("~/chess/issearchinggame")]
        public bool IsSearchingGame() =>
            usersQueue.Contains(CurrentUser);

        [HttpPost]
        [Route("~/chess/searchgame")]
        public string SearchGame()
        {
            if (usersQueue.Contains(CurrentUser) || matches.Any(x => x.ContainsUser(CurrentUser)))
                return "Player's playing the game or waiting for the game";

            usersQueue.Add(CurrentUser);

            if (usersQueue.Count == 2)
            {
                var game = new Game(10, 0);
                var match = new Match(usersQueue[1], usersQueue[0], game);
                game.TimedOut += () => SendGameState(match);
                matches.Add(match);

                foreach (var player in match.Players)
                {
                    var userId = GetUserId(player.Username);
                    hubContext.Clients.User(userId)
                        .StartGame();
                }
                usersQueue.Clear();
            }

            return "true";
        }

        [HttpPost]
        [Route("~/chess/offerdraw")]
        public bool OfferDraw()
        {


            return true;
        }

        [HttpPost]
        [Route("~/chess/resign")]
        public bool Resign()
        {


            return true;
        }

        [HttpPost]
        [Route("~/chess/getgamestate")]
        public string GetGameState()
        {
            var match = matches.FirstOrDefault(x => x.ContainsUser(CurrentUser));
            if (match is null)
                return string.Empty;

            var gameState = new GameState(match.Game, match.GetColor(CurrentUser));

            return ToJson(gameState);
        }

        [HttpPost]
        [Route("~/chess/makemove")]
        public string MakeMove([FromBody] JsonElement data)
        {
            var moveCode = data.Deserialize<string>();

            var match = matches.FirstOrDefault(x => x.ContainsUser(CurrentUser));

            if (match is null)
                return "Current user doesn't participate in any game";

            var isValidMove = match.Game.TryMakeMove(moveCode);
            
            if (!isValidMove)
                return "Invalid move";

            SendGameState(match);

            return string.Empty;
        }

        private void SendGameState(Match match)
        {
            foreach (var player in match.Players)
            {
                var userId = GetUserId(player.Username);
                var json = ToJson(new GameState(match.Game, player.Color));
                hubContext.Clients.User(userId).UpdateBoard(json);
            }
        }

        private static string ToJson(object obj)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(obj, jsonSettings);
            return json;
        }

        private string GetUserId(string username) =>
            context.Users.First(x => x.UserName == username).Id;
    }
}
