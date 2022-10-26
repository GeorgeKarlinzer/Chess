using Chess.Logic;

namespace Chess.Web.Hubs
{
    public interface IChessClient
    {
        Task UpdateBoard(string gameStateJson);

        Task StartGame();

        Task EndGame(string gameStateJson);
    }
}
