using Chess.Logic.Moves;
using static Chess.Logic.PlayerColor;
using static Chess.Logic.Positions;

namespace Chess.Logic.Pieces
{
    internal class King : Piece
    {
        public King(PlayerColor color, Vector2 position, int id, Board board) : base(color, position, id, board)
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
                var breakKingCastlePoss = Color.IsWhite() ? GetVectors("g1", "f1") : GetVectors("g8", "f8");
                var breakQueenCastlePoss = Color.IsWhite() ? GetVectors("b1", "c1", "d1") : GetVectors("b8", "c8", "d8");
                var kingRookPos = Color.IsWhite() ? VectorsMap["h1"] : VectorsMap["h8"];
                var queenRookPos = Color.IsWhite() ? VectorsMap["a1"] : VectorsMap["a8"];

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
