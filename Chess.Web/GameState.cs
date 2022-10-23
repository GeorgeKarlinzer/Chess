using Chess.Logic.Dtos;
using Newtonsoft.Json;

namespace Chess.Logic
{
    public class GameState
    {
        public List<PieceDto> Pieces { get; }
        public bool IsCheck { get; }
        public GameStatus Status { get; }
        public PlayerColor CurrentPlayer { get; }
        [JsonConverter(typeof(DictionaryWithEnumKeyConverter<PlayerColor, TimerDto>))]
        public Dictionary<PlayerColor, TimerDto> TimersMap { get; }

        public GameState(Game game, PlayerColor requester)
        {
            Pieces = game.GetPieces(requester);
            IsCheck = game.IsCheck;
            Status = game.Status;
            CurrentPlayer = game.CurrentPlayer;
            TimersMap = game.TimersMap;
        }
    }
}
