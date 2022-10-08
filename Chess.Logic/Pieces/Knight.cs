namespace Chess.Logic.Pieces
{
    internal class Knight : Piece
    {
        public Knight(PieceColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            base.CalculateMoves();

            Vector2[] deltaPositions = new Vector2[]
            {
                new(1, 2), new(-1, 2), new(2, 1), new(-2, 1),
                new(1, -2), new(-1, -2), new(2, -1), new(-2, -1)
            };

            foreach (var deltaPos in deltaPositions)
            {
                var pos = Position + deltaPos;

                if (!pos.IsValidChessPos())
                    continue;

                PossibleAttacks.Add(pos);

                if (game.CanBeat(pos, Color, out var attackedPiece) || game.CanMove(pos))
                {
                    PossibleMoves.Add(CreateMove(pos, attackedPiece));

                    if (attackedPiece is not null && attackedPiece.GetType() == typeof(King))
                    {
                        KingAttacks.Add(Position);
                        KingAttacks.Add(pos);
                    }
                }
            }
        }
    }
}
