using Chess.Logic;
using Chess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Chess.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChessController : Controller
    {
        private static Game game = new Game();
        private readonly ILogger<ChessController> _logger;

        static ChessController()
        {
            game.StartGame();
        }

        public ChessController(ILogger<ChessController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("~/chess/getpieces")]
        public object GetPieces()
        {
            var pieces = game.GetPieces();

            return pieces;
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
