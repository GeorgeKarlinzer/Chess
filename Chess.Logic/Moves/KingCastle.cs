using Chess.Logic.Pieces;
using static Chess.Logic.Positions;

namespace Chess.Logic.Moves
{
    internal class KingCastle : Move
    {
        private readonly Rook rook;

        public KingCastle(Piece piece, Rook rook, Game game) : 
            base(piece, null, piece.Color.IsWhite() ? VectorsMap["g1"] : VectorsMap["g8"], game)
        {
            this.rook = rook;
        }

        public override void MakeMove()
        {
            game.MovePiece(Piece, TargetPos, AttackedPiece);
            game.MovePiece(rook, rook.Position + new Vector2(-2, 0));
        }
    }
}
