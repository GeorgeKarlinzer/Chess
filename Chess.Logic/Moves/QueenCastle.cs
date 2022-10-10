using Chess.Logic.Pieces;
using static Chess.Logic.PlayerColor;
using static Chess.Logic.Positions;

namespace Chess.Logic.Moves
{
    internal class QueenCastle : Move
    {
        private readonly Rook rook;

        public QueenCastle(Piece piece, Rook rook, Board board) : 
            base(piece, null, piece.Color == White ? VectorsMap["c1"] : VectorsMap["c8"], board)
        {
            this.rook = rook;
        }

        public override void MakeMove()
        {
            board.MovePiece(Piece, TargetPos, AttackedPiece);
            board.MovePiece(rook, rook.Position + new Vector2(3, 0));
        }
    }
}
