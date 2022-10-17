using Newtonsoft.Json;

namespace Chess.Logic
{
    public class GameState
    {
        public List<PieceDto> Pieces { get; set; }
        public bool IsCheck { get; set; }
        public bool IsEnd { get; set; }
        public PlayerColor CurrentPlayer { get; set; }
        [JsonConverter(typeof(DictionaryWithEnumKeyConverter<PlayerColor, int>))]
        public Dictionary<PlayerColor, int> RemainTimes { get; set; }

        public GameState()
        {
            Pieces = new();
            RemainTimes = new();
        }
    }
}
