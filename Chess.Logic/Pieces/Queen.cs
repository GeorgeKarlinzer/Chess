namespace Chess.Logic.Pieces
{
    internal class Queen : Piece
    {
        public Queen(PlayerColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            base.CalculateMoves();

            AddContinuousMove(new(1, 0));
            AddContinuousMove(new(-1, 0));
            AddContinuousMove(new(0, 1));
            AddContinuousMove(new(0, -1));
            AddContinuousMove(new(1, 1));
            AddContinuousMove(new(-1, -1));
            AddContinuousMove(new(1, -1));
            AddContinuousMove(new(-1, 1));
        }
    }
}
