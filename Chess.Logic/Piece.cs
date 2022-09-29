namespace Chess.Logic
{
    internal class Piece
    {
        public int Id { get; }
        public PieceType Type { get; }
        public PieceColor Color { get; }
        public PositionEnum Position { get; }


        public Piece(PieceType type, PieceColor color, PositionEnum position, int id)
        {
            Type = type;
            Color = color;
            Position = position;
            Id = id;
        }

        public override string ToString() =>
            $"{Type} ({Color}): {Position}";
    }
}
