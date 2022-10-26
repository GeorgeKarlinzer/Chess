using Chess.Logic.Pieces;

namespace Chess.Logic.Moves
{
    internal class Promotion<TNewPiece> : Move where TNewPiece : Piece
    {
        public string PromotionPieceName { get; }

        public Promotion(Piece piece, Piece attackedPiece, Vector2 targetPos, Game game) : base(piece, attackedPiece, targetPos, game)
        {
            PromotionPieceName = typeof(TNewPiece).Name.ToLower();
            Code += $"{Piece.PieceCodesMap[typeof(TNewPiece)]}";
        }

        public override void MakeMove()
        {
            game.MovePiece(Piece, TargetPos, AttackedPiece);
            var newPiece = game.AddPiece<TNewPiece>(Piece.Color, Piece.Position);
            game.MovePiece(newPiece, TargetPos, Piece);
        }
    }
}
