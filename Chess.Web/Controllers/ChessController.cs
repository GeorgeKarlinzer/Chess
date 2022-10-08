using Chess.Logic;
using Chess.Logic.ExtensionMethods;
using Chess.Logic.Pieces;
using Chess.Models;
using Chess.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chess.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChessController : Controller
    {
        private static Game game = new Game();
        private readonly ILogger<ChessController> _logger;

        public ChessController(ILogger<ChessController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("~/chess/getpieces")]
        public object GetPieces()
        {
            var pieces = game.PiecesMap.Values;

            var data = pieces.Select(p => p.ToDto());

            return data;
        }

        [HttpPost]
        [Route("~/chess/makemove")]
        public object MakeMove([FromBody] JsonElement data)
        {
            var moveDto = data.Deserialize<MoveDto>();

            return GetPieces();
        }
    }
}
