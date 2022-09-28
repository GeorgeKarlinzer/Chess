namespace Chess.Logic
{
    internal class Piece
    {
        public PlayerColor Color { get; }
        public Cell Cell { get; }


        public Piece(PlayerColor color, Cell cell)
        {
            Color = color;
            Cell = cell;
        }
    }
}
