using Chess.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Chess.Server.Controllers
{
    public class MyHomeController : Controller
    {
        private static Match match;
        private static Piece selectedPiece;

        public MyHomeController()
        {
            match = new Match();
        }

        public IActionResult Index()
        {
            ViewBag.Pieces = match.PiecesMap;
            if (selectedPiece != null)
                ViewBag.AvailibleMoves = match.AvailibleMovesMap
                    .First(x => x.Key.Id == selectedPiece.Id).Value;
            return View();
        }

        //[HttpGet("id")]
        public IActionResult Moves()
        {
            var pos = (PositionEnum)int.Parse(Request.Query["id"]);
            selectedPiece = match.PiecesMap.Values.First(x => x.Position == pos);
            return RedirectToAction(nameof(Index));
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(int aa)
        //{
        //    var id = Request.Form["id"];
        //    selectedPiece = match.PiecesMap.Values.First(x => x.Id == id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
