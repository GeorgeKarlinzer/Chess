using Chess.Logic.Pieces;
using static Chess.Logic.PieceColor;
using static Chess.Logic.Positions;

namespace Chess.Logic.Moves
{
    internal class KingCastle : Move
    {
        private readonly Rook rook;

        public KingCastle(Piece piece, Rook rook, Board board) : base(piece, null, piece.Color == White ? G1 : G8, board)
        {
            this.rook = rook;
        }

        public override void MakeMove()
        {
            board.MovePiece(Piece, TargetPos, AttackedPiece);
            board.MovePiece(rook, rook.Position + new Vector2(-2, 0));
            board.CalculateAvailibleMoves(Piece.Color == White ? Black : White);
        }
    }
}
