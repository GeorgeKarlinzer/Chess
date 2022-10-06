using Chess.Logic;
using Chess.Logic.Pieces;
using Chess.Web;
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
                ViewBag.AvailibleMoves = selectedPiece.PossibleMoves;
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
            Game.MakeMove(selectedPiece, pos);

            return RedirectToAction(nameof(Index));
        }


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
