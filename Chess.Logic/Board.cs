using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Logic.PlayerColor;
using static Chess.Logic.Positions;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Chess.Logic.Moves;
using Chess.Logic.Pieces;
using System.Drawing;
using System.Xml.Serialization;

namespace Chess.Logic
{
    // TODO: Threefold repetition (en passant and castle)
    // TODO: No time with no pieces to mate opp
    internal class Board
    {
        public Clock Clock { get; }

        private int currentPieceId;
        private readonly Dictionary<string, int> repeatPositionMap;
        private readonly IPlayerSwitch playerSwitch;

        public PlayerColor CurrentPlayer { get; private set; }
        public GameStatus Status { get; private set; }
        public List<Move> Moves { get; }
        public Dictionary<Vector2, Piece> PiecesMap { get; }

        public bool IsCheck { get; private set; }

        public Board(int time, int bonus, IPlayerSwitch playerSwitch)
        {
            currentPieceId = 0;
            repeatPositionMap = new();
            PiecesMap = new();
            Moves = new();
            CurrentPlayer = White;
            this.playerSwitch = playerSwitch;
            Clock = new Clock(time, bonus, playerSwitch);

            Status = GameStatus.InProgress;

            GeneratePieces();

            CalculateAvailibleMoves();
        }

        private GameStatus GetStatus()
        {
            var hasMoves = PiecesMap.Values.Where(x => x.Color == CurrentPlayer)
                        .Any(x => x.PossibleMoves.Count > 0);

            if (IsCheck && !hasMoves)
            {
                if (CurrentPlayer == White)
                    return GameStatus.WhitesWon;
                else
                    return GameStatus.BlacksWon;
            }

            if (!hasMoves || repeatPositionMap.Any(x => x.Value == 3) || repeatPositionMap.Count == 100)
                return GameStatus.Draw;

            if (Clock.GetTime(White) == 0)
                return GameStatus.BlacksWon;

            if (Clock.GetTime(Black) == 0)
                return GameStatus.WhitesWon;

            return GameStatus.InProgress;
        }

        public void MovePiece(Piece piece, Vector2 targetPos, Piece attackedPiece = null)
        {
            PiecesMap.Remove(piece.Position);

            if (attackedPiece is not null)
            {
                PiecesMap.Remove(attackedPiece.Position);
            }
            if(attackedPiece is not null || piece.GetType() == typeof(Pawn))
                repeatPositionMap.Clear();

            PiecesMap[targetPos] = piece;
            piece.Position = targetPos;
            piece.IsMoved = true;
        }

        public void MakeMove(Move move)
        {
            Clock.Press();
            CurrentPlayer = playerSwitch.Switch(CurrentPlayer);

            move.MakeMove();

            CalculateAvailibleMoves();
            CalculateFenCode();

            Status = GetStatus();
            if (Status != GameStatus.InProgress)
                Clock.Stop();
        }

        public void CalculateFenCode()
        {
            var str = "";
            for (int y = 7; y >= 0; y--)
            {
                var emptyCells = 0;
                for (int x = 0; x < 8; x++)
                {
                    if (PiecesMap.TryGetValue(new(x, y), out var piece))
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

        #region Moves calculating

        public void CalculateAvailibleMoves()
        {
            PiecesMap.Values.Foreach(x => x.CalculateMoves());

            var king = PiecesMap.Values
                .First(x => x.GetType() == typeof(King) && x.Color == CurrentPlayer);

            var kingPos = king.Position;

            foreach (var enemyPiece in PiecesMap.Values.Where(x => x.Color != CurrentPlayer))
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
                    if (PiecesMap.TryGetValue(step, out var p))
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

            var attackedCells = PiecesMap.Values
                .Where(x => x.Color != CurrentPlayer)
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

            var attackedPieces = PiecesMap.Values
                .Where(x => x.Color != CurrentPlayer && x.KingAttacks.Count != 0)
                .ToList();

            IsCheck = attackedPieces.Any();

            if (attackedPieces.Count == 1)
            {
                var attackedPiece = attackedPieces.First();

                foreach (var piece in PiecesMap.Values.Where(x => x.Color == CurrentPlayer && x != king))
                {
                    piece.PossibleMoves.Where(x => !attackedPiece.KingAttacks.Contains(x.TargetPos))
                        .ToList()
                        .Foreach(x => piece.PossibleMoves.Remove(x));
                }
            }
        }

        public bool CanMove(Vector2 targetPos)
        {
            return targetPos.IsValidChessPos() && !PiecesMap.ContainsKey(targetPos);
        }

        public bool CanBeat(Vector2 targetPos, PlayerColor color, out Piece attackedPiece)
        {
            var canBeat = PiecesMap.TryGetValue(targetPos, out var piece) && piece.Color != color;
            attackedPiece = canBeat ? piece : null;
            return canBeat;
        }
        #endregion

        #region Generating
        private void GeneratePieces()
        {
            var pawnPos =  new[] { "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2" };

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

        public T AddPiece<T>(PlayerColor color, Vector2 position) where T : Piece
        {
            var piece = (T)Activator.CreateInstance(typeof(T), new object[] { color, position, currentPieceId++, this });
            PiecesMap[piece.Position] = piece;

            return piece;
        }
        #endregion
    }
}

// TODO: Add pawn metomorphosis
// TDOD: Add Timer
// TODO: Handle end of the game