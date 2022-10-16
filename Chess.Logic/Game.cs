using static Chess.Logic.Positions;

namespace Chess.Logic
{
    public class Game
    {
        private readonly Board board;
        private readonly PlayerSwitch playerSwitch;

        public List<PieceDto> Pieces => board.PiecesMap.Values.Select(x => x.ToDto(board.CurrentPlayer)).ToList();
        public bool IsCheck => board.IsCheck;
        public bool IsEnd => board.IsEnd;
        public PlayerColor CurrentPlayer => board.CurrentPlayer;
        public Dictionary<PlayerColor, int> RemainTimes => board.Clock.GetRemainTimes();

        public Game(int timeSec, int bonusSec)
        {
            playerSwitch = new PlayerSwitch();
            board = new Board(timeSec, bonusSec, playerSwitch);
        }

        /// <summary>
        /// Move code in format: {sourcePos}{targetPos}(optional){new piece code} Examples: 1) a3a4 2) c7c8Q
        /// </summary>
        public bool TryMakeMove(string moveCode)
        {
            if (moveCode.Length < 4 || board.IsEnd)
                return false;

            var sourcePosCode = new string(new[] { moveCode[0], moveCode[1] });

            if (!sourcePosCode.IsValidChessPos())
                return false;

            var sourcePos = VectorsMap[sourcePosCode];
            var shortCode = moveCode.Remove(0, 2);

            var piece = board.PiecesMap.Values.FirstOrDefault(x => x.Position == sourcePos && x.Color == board.CurrentPlayer);
            var move = piece?.PossibleMoves.FirstOrDefault(x => x.Code == shortCode);

            if (move is null)
                return false;

            board.MakeMove(move);

            return true;
        }
    }
}
