using Chess.Logic.Dtos;
using Chess.Logic.Moves;
using Chess.Logic.Pieces;
using static Chess.Logic.PlayerColor;
using static Chess.Logic.Positions;

namespace Chess.Logic
{
    public class Game
    {
        private int currentPieceId;
        private GameStatus _status;

        private readonly PlayerSwitch playerSwitch;
        private readonly Clock clock;
        private readonly Dictionary<string, int> repeatPositionMap;
        internal readonly List<Move> moves;
        internal readonly Dictionary<Vector2, Piece> piecesMap;

        public event Action OnGameEnd;

        public GameStatus Status
        {
            get => _status;
            set
            {
                if (_status == GameStatus.InProgress && value != GameStatus.InProgress)
                {
                    _status = value;
                    clock.Stop();
                    OnGameEnd?.Invoke();
                    OnGameEnd = null;
                }
            }
        }
        public bool IsCheck { get; private set; }
        public Dictionary<PlayerColor, TimerDto> TimersMap => clock.GetTimersMap();

        public Game(int timeSec, int bonusSec)
        {
            playerSwitch = new PlayerSwitch(PlayerColor.White);
            currentPieceId = 0;
            repeatPositionMap = new();
            piecesMap = new();
            moves = new();
            clock = new Clock(timeSec, bonusSec, playerSwitch);
            clock.TimedOut += Resign;
            _ = clock.RunTimer();

            _status = GameStatus.InProgress;

            GeneratePieces();

            CalculateAvailibleMoves();
        }

        #region API
        public void Resign(PlayerColor player)
        {
            Status = player == White ? GameStatus.BlacksWon : GameStatus.WhitesWon;
        }

        public void MakeDraw()
        {
            Status = GameStatus.Draw;
        }

        public List<PieceDto> GetPieces(PlayerColor requester)
        {
            var pieces = piecesMap.Values
                .Select(x => x.ToDto(requester, playerSwitch.CurrentPlayer))
                .ToList();

            if (Status != GameStatus.InProgress)
                pieces.Foreach(x => x.Moves.Clear());

            return pieces;
        }

        /// <summary>
        /// Move code in format: {sourcePos}{targetPos}(optional){new piece code} Examples: 1) a3a4 2) c7c8Q
        /// </summary>
        public bool TryMakeMove(string moveCode)
        {
            if (moveCode.Length < 4 || Status != GameStatus.InProgress)
                return false;

            var sourcePosCode = new string(new[] { moveCode[0], moveCode[1] });

            if (!sourcePosCode.IsValidChessPos())
                return false;

            var sourcePos = VectorsMap[sourcePosCode];
            var shortCode = moveCode.Remove(0, 2);

            var piece = piecesMap.Values.FirstOrDefault(x => x.Position == sourcePos && x.Color == playerSwitch.CurrentPlayer);
            var move = piece?.PossibleMoves.FirstOrDefault(x => x.Code == shortCode);

            if (move is null)
                return false;

            MakeMove(move);

            return true;
        }
        #endregion

        #region Modifyings

        private GameStatus GetStatus()
        {
            var hasMoves = piecesMap.Values.Where(x => x.Color == playerSwitch.CurrentPlayer)
                        .Any(x => x.PossibleMoves.Count > 0);

            if (IsCheck && !hasMoves)
            {
                if (playerSwitch.CurrentPlayer == White)
                    return GameStatus.WhitesWon;
                else
                    return GameStatus.BlacksWon;
            }

            if (!hasMoves || repeatPositionMap.Any(x => x.Value == 3) || repeatPositionMap.Count == 100)
                return GameStatus.Draw;

            if (clock.GetTime(White) == 0)
                return GameStatus.BlacksWon;

            if (clock.GetTime(Black) == 0)
                return GameStatus.WhitesWon;

            return GameStatus.InProgress;
        }

        internal void MovePiece(Piece piece, Vector2 targetPos, Piece attackedPiece = null)
        {
            piecesMap.Remove(piece.Position);

            if (attackedPiece is not null)
            {
                piecesMap.Remove(attackedPiece.Position);
            }
            if (attackedPiece is not null || piece.GetType() == typeof(Pawn))
                repeatPositionMap.Clear();

            piecesMap[targetPos] = piece;
            piece.Position = targetPos;
            piece.IsMoved = true;
        }

        private void MakeMove(Move move)
        {
            playerSwitch.Switch();

            move.MakeMove();

            CalculateAvailibleMoves();
            CalculateFenCode();

            Status = GetStatus();
            if (Status != GameStatus.InProgress)
                clock.Stop();

            moves.Add(move);
        }

        internal void CalculateFenCode()
        {
            var str = "";
            for (int y = 7; y >= 0; y--)
            {
                var emptyCells = 0;
                for (int x = 0; x < 8; x++)
                {
                    if (piecesMap.TryGetValue(new(x, y), out var piece))
                    {
                        str += piece.FENCode;

                        if (emptyCells != 0)
                            str += emptyCells.ToString();

                        emptyCells = 0;
                    }
                    else
                        emptyCells++;
                }
                if (emptyCells != 0)
                    str += emptyCells.ToString();

                str += "/";
            }

            if (!repeatPositionMap.ContainsKey(str))
                repeatPositionMap[str] = 1;
            else
                repeatPositionMap[str]++;
        }

        #endregion

        #region Moves calculating

        internal void CalculateAvailibleMoves()
        {
            piecesMap.Values.Foreach(x => x.CalculateMoves());

            var king = piecesMap.Values
                .First(x => x.GetType() == typeof(King) && x.Color == playerSwitch.CurrentPlayer);

            var kingPos = king.Position;

            foreach (var enemyPiece in piecesMap.Values.Where(x => x.Color != playerSwitch.CurrentPlayer))
            {
                var type = enemyPiece.GetType();
                var enemyPos = enemyPiece.Position;
                var deltaPos = kingPos - enemyPos;

                if ((type != typeof(Bishop) && type != typeof(Rook) && type != typeof(Queen))
                    || !enemyPos.IsSameLine(kingPos))
                    continue;

                Piece coverPiece = null;

                deltaPos = deltaPos.ToUnitDirection();

                var step = enemyPos;
                while ((step += deltaPos) != kingPos)
                {
                    if (piecesMap.TryGetValue(step, out var p))
                    {
                        if (p.Color == enemyPiece.Color || coverPiece != null)
                        {
                            coverPiece = null;
                            break;
                        }
                        else
                        {
                            coverPiece = p;
                        }
                    }
                }

                if (coverPiece != null)
                {
                    coverPiece.PossibleMoves
                        .Where(x => !x.TargetPos.IsBetween(kingPos, enemyPos) && x.TargetPos != enemyPos)
                        .ToList()
                        .Foreach(x => coverPiece.PossibleMoves.Remove(x));
                }
            }

            var attackedCells = piecesMap.Values
                .Where(x => x.Color != playerSwitch.CurrentPlayer)
                .SelectMany(x => x.PossibleAttacks)
                .Distinct()
                .ToList();

            king.PossibleMoves.Where(x => attackedCells.Contains(x.TargetPos))
                .ToList()
                .Foreach(x => king.PossibleMoves.Remove(x));

            var kingCastle = king.PossibleMoves.FirstOrDefault(x => x.GetType() == typeof(KingCastle));
            var queenCastle = king.PossibleMoves.FirstOrDefault(x => x.GetType() == typeof(QueenCastle));

            var breakKingCastlePoss = king.Color.IsWhite() ? GetVectors("e1", "f1") : GetVectors("e8", "f8");
            var breakQueenCastlePoss = king.Color.IsWhite() ? GetVectors("e1", "d1") : GetVectors("e8", "d8");

            if (kingCastle is not null && attackedCells.Any(x => breakKingCastlePoss.Contains(x)))
            {
                king.PossibleMoves.Remove(kingCastle);
            }

            if (queenCastle is not null && attackedCells.Any(x => breakQueenCastlePoss.Contains(x)))
            {
                king.PossibleMoves.Remove(queenCastle);
            }

            var attackedPieces = piecesMap.Values
                .Where(x => x.Color != playerSwitch.CurrentPlayer && x.KingAttacks.Count != 0)
                .ToList();

            IsCheck = attackedPieces.Any();

            if (attackedPieces.Count == 1)
            {
                var attackedPiece = attackedPieces.First();

                foreach (var piece in piecesMap.Values.Where(x => x.Color == playerSwitch.CurrentPlayer && x != king))
                {
                    piece.PossibleMoves.Where(x => !attackedPiece.KingAttacks.Contains(x.TargetPos))
                        .ToList()
                        .Foreach(x => piece.PossibleMoves.Remove(x));
                }
            }
        }

        internal bool CanMove(Vector2 targetPos)
        {
            return targetPos.IsValidChessPos() && !piecesMap.ContainsKey(targetPos);
        }

        internal bool CanBeat(Vector2 targetPos, PlayerColor color, out Piece attackedPiece)
        {
            var canBeat = piecesMap.TryGetValue(targetPos, out var piece) && piece.Color != color;
            attackedPiece = canBeat ? piece : null;
            return canBeat;
        }
        #endregion

        #region Generating
        private void GeneratePieces()
        {
            var pawnPos = new[] { "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2" };

            foreach (var pos in pawnPos)
            {
                var v = VectorsMap[pos];

                AddPiece<Pawn>(White, v);
                AddPiece<Pawn>(Black, new(v.X, v.Y + 5));
            }

            AddPiece<Rook>(White, VectorsMap["a1"]);
            AddPiece<Rook>(White, VectorsMap["h1"]);
            AddPiece<Rook>(Black, VectorsMap["a8"]);
            AddPiece<Rook>(Black, VectorsMap["h8"]);

            AddPiece<Knight>(White, VectorsMap["b1"]);
            AddPiece<Knight>(White, VectorsMap["g1"]);
            AddPiece<Knight>(Black, VectorsMap["b8"]);
            AddPiece<Knight>(Black, VectorsMap["g8"]);

            AddPiece<Bishop>(White, VectorsMap["c1"]);
            AddPiece<Bishop>(White, VectorsMap["f1"]);
            AddPiece<Bishop>(Black, VectorsMap["c8"]);
            AddPiece<Bishop>(Black, VectorsMap["f8"]);

            AddPiece<Queen>(White, VectorsMap["d1"]);
            AddPiece<Queen>(Black, VectorsMap["d8"]);

            AddPiece<King>(White, VectorsMap["e1"]);
            AddPiece<King>(Black, VectorsMap["e8"]);
        }

        internal T AddPiece<T>(PlayerColor color, Vector2 position) where T : Piece
        {
            var piece = (T)Activator.CreateInstance(typeof(T), new object[] { color, position, currentPieceId++, this });
            piecesMap[piece.Position] = piece;

            return piece;
        }
        #endregion
    }
}
