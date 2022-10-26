using Chess.Logic.Moves;
using static Chess.Logic.PlayerColor;
using static Chess.Logic.Positions;

namespace Chess.Logic.Pieces
{
    internal class King : Piece
    {
        public King(PlayerColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
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

                if (game.CanBeat(pos, Color, out var attackedPiece) || game.CanMove(pos))
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

                if (game.piecesMap.TryGetValue(kingRookPos, out var kingRook) && !kingRook.IsMoved
                    && !game.piecesMap.Keys.Any(x => breakKingCastlePoss.Contains(x)))
                {
                    var move = new KingCastle(this, kingRook as Rook, game);
                    PossibleMoves.Add(move);
                }

                if (game.piecesMap.TryGetValue(queenRookPos, out var queenRook) && !queenRook.IsMoved
                    && !game.piecesMap.Keys.Any(x => breakQueenCastlePoss.Contains(x)))
                {
                    var move = new QueenCastle(this, queenRook as Rook, game);
                    PossibleMoves.Add(move);
                }
            }
        }
    }
}
