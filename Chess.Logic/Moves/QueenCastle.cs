using Chess.Logic.Pieces;
using static Chess.Logic.PlayerColor;
using static Chess.Logic.Positions;

namespace Chess.Logic.Moves
{
    internal class QueenCastle : Move
    {
        private readonly Rook rook;

        public QueenCastle(Piece piece, Rook rook, Game game) : 
            base(piece, null, piece.Color == White ? VectorsMap["c1"] : VectorsMap["c8"], game)
        {
            this.rook = rook;
        }

        public override void MakeMove()
        {
            game.MovePiece(Piece, TargetPos, AttackedPiece);
            game.MovePiece(rook, rook.Position + new Vector2(3, 0));
        }
    }
}
