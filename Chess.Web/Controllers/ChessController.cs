using Chess.Logic;
using Chess.Logic.Pieces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
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
        [Route("~/chess/getsomenew")]
        public IEnumerable<object> GetSomeNew()
        {
            var pieces = game.PiecesMap.Values;

            var data = pieces.Select(p => new
            {
                x = p.Position.X,
                y = p.Position.Y,
                name = p.GetType().Name,
                color = p.Color.ToString(),
                moves = p.PossibleMoves.Select(m => new { x = m.TargetPos.X, y = m.TargetPos.Y })
            });

            return data;
        }
    }
}
