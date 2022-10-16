using Chess.Logic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace Chess.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChessController : Controller
    {
        private static Game game = GetNewGame();
        private readonly ILogger<ChessController> _logger;

        public ChessController(ILogger<ChessController> logger)
        {
            _logger = logger;
        }

        private static Game GetNewGame() =>
            new(3 * 60, 10);

        [HttpGet]
        [Route("~/chess/getpieces")]
        public object GetPieces()
        {
            var gameState = new GameState()
            {
                Pieces = game.Pieces,
                IsCheck = game.IsCheck,
                IsEnd = game.IsEnd,
                CurrentPlayer = game.CurrentPlayer,
                RemainTimes = game.RemainTimes
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(gameState, jsonSettings);

            return json;
        }

        [HttpPost]
        [Route("~/chess/makemove")]
        public object MakeMove([FromBody] JsonElement data)
        {
            var moveCode = data.Deserialize<string>();

            var res = game.TryMakeMove(moveCode);

            return GetPieces();
        }

        [HttpPost]
        [Route("~/chess/restart")]
        public void RestartGame()
        {
            game = GetNewGame();
        }
    }
}
