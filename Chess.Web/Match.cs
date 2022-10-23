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
            Players.Add(new(blackUsername, PlayerColor.Black));
            Players.Add(new(whiteUsername, PlayerColor.White));
        }

        public bool ContainsUser(string username)
            => Players.Any(x => x.Username == username);

        public bool IsPlayersTurn(string username) =>
            Game.CurrentPlayer == Players.First(x => x.Username == username).Color;

        public PlayerColor GetColor(string username) =>
            Players.First(x => x.Username == username).Color;
    }
}
