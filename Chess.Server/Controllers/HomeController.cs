using Chess.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Chess.Server.Controllers
{
    public class MyHomeController : Controller
    {
        private static Game game = new Game();
        private static Piece selectedPiece;

        public MyHomeController()
        {
        }

        public IActionResult Index()
        {
            ViewBag.Pieces = game.PiecesMap;

            if (selectedPiece != null)
            {
                ViewBag.AvailibleMoves = game.AvailibleMovesMap[selectedPiece];
            }    

            return View();
        }

        public IActionResult Moves()
        {
            var id = int.Parse(Request.Query["id"]);
            selectedPiece = game.PiecesMap.Values.First(x => x.Id == id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Move()
        {
            var pos = int.Parse(Request.Query["cell"]).ToChessPos();
            game.MakeMove(selectedPiece, pos);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
