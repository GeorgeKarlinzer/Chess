namespace Chess.Logic.Pieces
{
    internal class Knight : Piece
    {
        public Knight(PieceColor color, Vector2 position, int id, Board board) : base(color, position, id, board)
        {
            FENCode = color == PieceColor.White ? 'N' : 'n';
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

                if (board.CanBeat(pos, Color, out var attackedPiece) || board.CanMove(pos))
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
