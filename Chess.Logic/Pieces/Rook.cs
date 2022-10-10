namespace Chess.Logic.Pieces
{
    internal class Rook : Piece
    {
        public Rook(PlayerColor color, Vector2 position, int id, Board board) : base(color, position, id, board)
        {
        }

        public override void CalculateMoves()
        {
            base.CalculateMoves();

            AddContinuousMove(new(1, 0));
            AddContinuousMove(new(-1, 0));
            AddContinuousMove(new(0, 1));
            AddContinuousMove(new(0, -1));
        }
    }
}
