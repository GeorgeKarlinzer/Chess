using Chess.Logic;
using Chess.Logic.Pieces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public IEnumerable<Models.Piece> GetSomeNew()
        {
            var pieces = game.PiecesMap.Values;

            var data = new List<Models.Piece>();
            foreach (var piece in pieces)
            {
                data.Add(new Models.Piece() { 
                    Name = piece.GetType().Name, 
                    Color = piece.Color.ToString(),
                    X = piece.Position.X,
                    Y = piece.Position.Y
                });
            }

            return data;
        }
    }
}
