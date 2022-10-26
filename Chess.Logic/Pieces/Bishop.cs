namespace Chess.Logic.Pieces
{
    internal class Bishop : Piece
    {
        public Bishop(PlayerColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            base.CalculateMoves();

            AddContinuousMove(new(1, 1));
            AddContinuousMove(new(-1, -1));
            AddContinuousMove(new(1, -1));
            AddContinuousMove(new(-1, 1));
        }
    }
}
