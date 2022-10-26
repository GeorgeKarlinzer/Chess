using Chess.Logic.Dtos;
using Newtonsoft.Json;

namespace Chess.Logic
{
    public class GameState
    {
        public List<PieceDto> Pieces { get; }
        public bool IsCheck { get; }
        public GameStatus Status { get; }
        public PlayerColor Player { get; }
        [JsonConverter(typeof(DictionaryWithEnumKeyConverter<PlayerColor, TimerDto>))]
        public Dictionary<PlayerColor, TimerDto> TimersMap { get; }

        public GameState(Game game, PlayerColor requester)
        {
            Pieces = game.GetPieces(requester);
            IsCheck = game.IsCheck;
            Status = game.Status;
            Player = requester;
            TimersMap = game.TimersMap;
        }
    }
}
