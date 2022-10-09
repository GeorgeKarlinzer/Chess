using Chess.Logic.Pieces;
using static Chess.Logic.PieceColor;

namespace Chess.Logic.Moves
{
    internal class Move
    {
        protected readonly Board board;

        public Piece Piece { get; }
        public Piece AttackedPiece { get; }
        public Vector2 SourcePos { get; }
        public Vector2 TargetPos { get; }
        public string Code { get; protected set; }

        public Move(Piece piece, Piece attackedPiece, Vector2 targetPos, Board board)
        {
            Piece = piece;
            AttackedPiece = attackedPiece;
            TargetPos = targetPos;
            SourcePos = piece.Position;
            this.board = board;
            Code = $"{Positions.PositionsMap[TargetPos]}";
        }

        public virtual void MakeMove()
        {
            board.MovePiece(Piece, TargetPos, AttackedPiece);
        }
    }
}
