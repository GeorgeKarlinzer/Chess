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
        private static Game game = new();
        private readonly ILogger<ChessController> _logger;

        static ChessController()
        {
        }

        public ChessController(ILogger<ChessController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("~/chess/getpieces")]
        public object GetPieces()
        {
            var state = game.GetGameState();
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(new { pieces = state.pieces, isCheck = state.isCheck, isEnd = state.isEnd }, jsonSettings);

            return json;
        }

        [HttpPost]
        [Route("~/chess/makemove")]
        public object MakeMove([FromBody] JsonElement data)
        {
            var moveDto = data.Deserialize<MoveDto>();

            game.MakeMove(moveDto);

            return GetPieces();
        }
    }
}
