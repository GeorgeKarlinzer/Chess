using Chess.Logic;
using Chess.Web.Models;

namespace Chess.Web
{
    public class Match
    {
        public List<Player> Players { get; }
        public Game Game { get; }

        public Match(string blackUsername, string whiteUsername, Game game)
        {
            Players = new();
            Game = game;
            var rand = new Random().Next(0, 2);
            Players.Add(new(blackUsername, (PlayerColor)rand));
            Players.Add(new(whiteUsername, (PlayerColor)(1 - rand)));
        }

        public bool ContainsUser(string username)
            => Players.Any(x => x.Username == username);

        public PlayerColor GetColor(string username) =>
            Players.First(x => x.Username == username).Color;
    }
}
