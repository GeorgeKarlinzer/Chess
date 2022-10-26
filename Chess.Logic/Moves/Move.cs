using Chess.Logic.Pieces;
using static Chess.Logic.PlayerColor;

namespace Chess.Logic.Moves
{
    internal class Move
    {
        protected readonly Game game;

        public Piece Piece { get; }
        public Piece AttackedPiece { get; }
        public Vector2 SourcePos { get; }
        public Vector2 TargetPos { get; }
        public string Code { get; protected set; }

        public Move(Piece piece, Piece attackedPiece, Vector2 targetPos, Game game)
        {
            Piece = piece;
            AttackedPiece = attackedPiece;
            TargetPos = targetPos;
            SourcePos = piece.Position;
            this.game = game;
            Code = $"{Positions.PositionsMap[TargetPos]}";
        }

        public virtual void MakeMove()
        {
            game.MovePiece(Piece, TargetPos, AttackedPiece);
        }
    }
}
