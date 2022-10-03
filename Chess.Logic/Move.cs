namespace Chess.Logic
{
    public class Move
    {
        public int Id { get; }
        public Piece Piece { get; }
        public Vector2 SourcePos { get; }
        public Vector2 TargetPos { get; }

        public Move(int id, Piece piece, Vector2 sourcePos, Vector2 targetPos)
        {
            Id = id;
            Piece = piece;
            TargetPos = targetPos;
            SourcePos = sourcePos;
        }
    }
}
