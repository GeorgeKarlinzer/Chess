using Chess.Logic;
using Chess.Web.Data;
using Chess.Web.Hubs;
using Chess.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;
using System.Text.Json;

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

        //[HttpGet]
        //[Route("~/chess/getpieces")]
        //public object GetPieces()
        //{
        //    var gameState = new GameState()
        //    {
        //        Pieces = game.Pieces,
        //        IsCheck = game.IsCheck,
        //        IsEnd = game.IsEnd,
        //        CurrentPlayer = game.CurrentPlayer,
        //        RemainTimes = game.RemainTimes
        //    };

        //    var jsonSettings = new JsonSerializerSettings
        //    {
        //        ContractResolver = new CamelCasePropertyNamesContractResolver()
        //    };

        //    var json = JsonConvert.SerializeObject(gameState, jsonSettings);

        //    return json;
        //}

        [HttpPost]
        [Route("~/chess/searchgame")]
        public string SearchGame()
        {
            if (usersQueue.Contains(CurrentUser) || matches.Any(x => x.ContainsUser(CurrentUser)))
                return "Player's playing the game or waiting for the game";

            usersQueue.Add(CurrentUser);

            if (usersQueue.Count == 2)
            {
                var game = new Game(3 * 60, 2);
                var match = new Match(usersQueue[1], usersQueue[0], game);
                matches.Add(match);

                foreach (var player in match.Players)
                {
                    var userId = GetUserId(player.Username);
                    hubContext.Clients.User(userId)
                        .StartGame(player.Color);
                }
                usersQueue.Clear();
            }

            return string.Empty;
        }

        [HttpPost]
        [Route("~/chess/getgamestate")]
        public string GetGameState()
        {
            var match = matches.FirstOrDefault(x => x.ContainsUser(CurrentUser));
            if (match is null)
                return "Current user doesn't participate in any game";

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

            if (!match.IsPlayersTurn(CurrentUser))
                return "Invalid move";

            var isValidMove = match.Game.TryMakeMove(moveCode);
            
            if (!isValidMove)
                return "Invalid move";

            var players = match.Players.Select(x => x.Username);

            foreach (var player in match.Players)
            {
                var userId = GetUserId(player.Username);
                var json = ToJson(new GameState(match.Game, player.Color));
                hubContext.Clients.User(userId).UpdateBoard(json);
            }

            return string.Empty;
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
