using Chess.Logic;
using Chess.Models;
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
        private static Game game = new(3 * 60, 0);
        private readonly ILogger<ChessController> _logger;

        public ChessController(ILogger<ChessController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("~/chess/getpieces")]
        public object GetPieces()
        {
            var (pieces, isCheck, isEnd, blackTime, whiteTime, currentPlayer) = game.GetGameState();
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var data = new { pieces, isCheck, isEnd, blackTime, whiteTime, currentPlayer };

            var json = JsonConvert.SerializeObject(data, jsonSettings);

            return json;
        }

        [HttpPost]
        [Route("~/chess/makemove")]
        public object MakeMove([FromBody] JsonElement data)
        {
            var moveCode = data.Deserialize<string>();

            game.MakeMove(moveCode);

            return GetPieces();
        }

        [HttpPost]
        [Route("~/chess/restart")]
        public void RestartGame()
        {
            game = new(3 * 60, 0);
        }
    }
}
