using Chess.Logic.Pieces;

namespace Chess.Logic.Moves
{
    internal class Promotion<TNewPiece> : Move where TNewPiece : Piece
    {
        public string PromotionPieceName { get; }

        public Promotion(Piece piece, Piece attackedPiece, Vector2 targetPos, Board board) : base(piece, attackedPiece, targetPos, board)
        {
            PromotionPieceName = typeof(TNewPiece).Name.ToLower();
        }

        public override void MakeMove()
        {
            board.MovePiece(Piece, TargetPos, AttackedPiece);
            var newPiece = board.AddPiece<TNewPiece>(Piece.Color, Piece.Position);
            board.MovePiece(newPiece, TargetPos, Piece);
        }

        public override bool IsThisMove(Vector2 targetPos, object parameter)
        {
            return TargetPos == targetPos && PromotionPieceName == parameter?.ToString();
        }
    }
}
