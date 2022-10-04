using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Logic.PieceColor;
using static Chess.Logic.Positions;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Chess.Logic.Moves;
using Chess.Logic.Pieces;
using System.Drawing;

namespace Chess.Logic
{
    public class Game
    {
        public List<Move> Moves { get; }

        public Dictionary<Vector2, Piece> PiecesMap { get; }

        public Game()
        {
            PiecesMap = new();

            Moves = new List<Move>();

            GeneratePieces();

            CalculateAvailibleMoves(White);
        }

        public void MakeMove(Piece piece, Vector2 targetPos)
        {
            piece.PossibleMoves
                .FirstOrDefault(x => x.TargetPos == targetPos)
                ?.MakeMove();
        }

        #region Moves calculating

        internal void CalculateAvailibleMoves(PieceColor color)
        {
            PiecesMap.Values.Foreach(x => x.CalculateMoves());

            var king = PiecesMap.Values
                .First(x => x.GetType() == typeof(King) && x.Color == color);

            var kingPos = king.Position;

            foreach (var enemyPiece in PiecesMap.Values.Where(x => x.Color != color))
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
                        .Where(x => !x.TargetPos.IsBetween(kingPos, enemyPos))
                        .ToList()
                        .Foreach(x => coverPiece.PossibleMoves.Remove(x));
                }
            }

            var attackedPieces = PiecesMap.Values
                .Where(x => x.Color != color && x.KingAttacks.Count != 0)
                .ToList();

            var attackedCells = PiecesMap.Values
                .Where(x => x.Color != color)
                .SelectMany(x => x.PossibleAttacks)
                .Union(attackedPieces.SelectMany(x => x.KingAttacks))
                .Distinct()
                .ToList();

            var aaa = king.PossibleMoves.Where(x => attackedCells.Contains(x.TargetPos))
                .ToList();
            aaa.Foreach(x => king.PossibleMoves.Remove(x));

            if(attackedPieces.Count == 1)
            {
                var attackedPiece = attackedPieces.First();

                foreach (var piece in PiecesMap.Values.Where(x => x.Color == color && x != king))
                {
                    piece.PossibleMoves.Where(x => !attackedPiece.KingAttacks.Contains(x.TargetPos))
                        .ToList()
                        .Foreach(x => piece.PossibleMoves.Remove(x));
                }
            }
        }

        internal bool CanMove(Vector2 targetPos)
        {
            return targetPos.IsValidChessPos() && !PiecesMap.ContainsKey(targetPos);
        }

        internal bool CanBeat(Vector2 targetPos, PieceColor color, out Piece attackedPiece)
        {
            var canBeat = PiecesMap.TryGetValue(targetPos, out var piece) && piece.Color != color;
            attackedPiece = canBeat ? piece : null;
            return canBeat;
        }
        #endregion

        #region Generating
        private void GeneratePieces()
        {
            var pawnPos = new[] { A2, B2, C2, D2, E2, F2, G2, H2 };

            foreach (var pos in pawnPos)
            {
                AddPiece<Pawn>(White, pos);
                AddPiece<Pawn>(Black, new(pos.X, pos.Y + 5));
            }

            AddPiece<Rook>(White, A1);
            AddPiece<Rook>(White, H1);
            AddPiece<Rook>(Black, A8);
            AddPiece<Rook>(Black, H8);

            AddPiece<Knight>(White, B1);
            AddPiece<Knight>(White, G1);
            AddPiece<Knight>(Black, B8);
            AddPiece<Knight>(Black, G8);

            AddPiece<Bishop>(White, C1);
            AddPiece<Bishop>(White, F1);
            AddPiece<Bishop>(Black, C8);
            AddPiece<Bishop>(Black, F8);

            AddPiece<Queen>(White, D1);
            AddPiece<Queen>(Black, D8);

            AddPiece<King>(White, E1);
            AddPiece<King>(Black, E8);
        }

        private void AddPiece<T>(PieceColor color, Vector2 position) where T : Piece
        {
            var piece = (T)Activator.CreateInstance(typeof(T), new object[] { color, position, PiecesMap.Count, this });
            //var piece = new T(color, position, PiecesMap.Count);
            PiecesMap[piece.Position] = piece;
        }
        #endregion
    }
}
