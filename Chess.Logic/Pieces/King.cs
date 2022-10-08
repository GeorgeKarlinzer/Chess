using Chess.Logic.Moves;
using static Chess.Logic.PieceColor;
using static Chess.Logic.Positions;

namespace Chess.Logic.Pieces
{
    internal class King : Piece
    {
        public King(PieceColor color, Vector2 position, int id, Board board) : base(color, position, id, board)
        {
        }

        public override void CalculateMoves()
        {
            base.CalculateMoves();

            Vector2[] deltaPositions = new Vector2[]
            {
                new(1, 0), new(0, 1), new(1, -1), new(1, 1),
                new(-1, 0), new(0, -1), new(-1, 1), new(-1, -1)
            };

            foreach (var deltaPos in deltaPositions)
            {
                var pos = Position + deltaPos;

                if (pos.IsValidChessPos())
                    PossibleAttacks.Add(pos);

                if (board.CanBeat(pos, Color, out var attackedPiece) || board.CanMove(pos))
                {
                    PossibleMoves.Add(CreateMove(pos, attackedPiece));
                }
            }

            if (!IsMoved)
            {
                var breakKingCastlePoss = Color == White ? new[] { G1, F1 } : new[] { G8, F8 };
                var breakQueenCastlePoss = Color == White ? new[] { B1, C1, D1 } : new[] { B8, C8, D8 };
                var kingRookPos = Color == White ? H1 : H8;
                var queenRookPos = Color == White ? A1 : A8;

                if (board.PiecesMap.TryGetValue(kingRookPos, out var kingRook) && !kingRook.IsMoved
                    && !board.PiecesMap.Keys.Any(x => breakKingCastlePoss.Contains(x)))
                {
                    var move = new KingCastle(this, kingRook as Rook, board);
                    PossibleMoves.Add(move);
                }

                if (board.PiecesMap.TryGetValue(queenRookPos, out var queenRook) && !queenRook.IsMoved
                    && !board.PiecesMap.Keys.Any(x => breakQueenCastlePoss.Contains(x)))
                {
                    var move = new QueenCastle(this, queenRook as Rook, board);
                    PossibleMoves.Add(move);
                }
            }
        }
    }
}
