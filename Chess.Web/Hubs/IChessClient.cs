using Chess.Logic;

namespace Chess.Web.Hubs
{
    public interface IChessClient
    {
        Task UpdateBoard(GameState gameState);

        Task StartGame(PlayerColor color);

        Task EndGame(GameState gameState);
    }
}
