namespace Chess.Logic
{
    public class Piece
    {
        public int Id { get; }
        public PieceType Type { get; }
        public PieceColor Color { get; }
        public Vector2 Position { get; set; }

        public List<Vector2> PossibleMoves { get; }
        public List<Vector2> PossibleAttacks { get; }

        public Piece(PieceType type, PieceColor color, Vector2 position, int id)
        {
            Type = type;
            Color = color;
            Position = position;
            Id = id;
        }

        public override string ToString() =>
            $"{Type} ({Color}): {Position.ToChessCoord()}";
    }
}
