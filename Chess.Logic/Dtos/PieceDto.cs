namespace Chess.Logic
{
    public class PieceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PlayerColor Color { get; set; }
        public Vector2 Position { get; set; }
        public List<Vector2> Moves { get; set; }
    }
}
