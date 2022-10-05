using Chess.Logic.Pieces;
using static Chess.Logic.PieceColor;
using static Chess.Logic.Positions;

namespace Chess.Logic.Moves
{
    public class Move
    {
        protected readonly Game game;

        public Piece Piece { get; }
        public Piece AttackedPiece { get; }
        public Vector2 SourcePos { get; }
        public Vector2 TargetPos { get; }

        public Move(Piece piece, Piece attackedPiece, Vector2 targetPos, Game game)
        {
            Piece = piece;
            AttackedPiece = attackedPiece;
            TargetPos = targetPos;
            SourcePos = piece.Position;
            this.game = game;
        }

        public virtual void MakeMove()
        {
            game.MovePiece(Piece, TargetPos, AttackedPiece);
            game.CalculateAvailibleMoves(Piece.Color == White ? Black : White);
        }
    }

    public class KingCastle : Move
    {
        private readonly Rook rook;

        public KingCastle(Piece piece, Rook rook, Game game) : base(piece, null, piece.Color == White ? G1 : G8, game)
        {
            this.rook = rook;
        }

        public override void MakeMove()
        {
            game.MovePiece(Piece, TargetPos, AttackedPiece);
            game.MovePiece(rook, rook.Position + new Vector2(-2, 0));
            game.CalculateAvailibleMoves(Piece.Color == White ? Black : White);
        }
    }

    public class QueenCastle : Move
    {
        private readonly Rook rook;

        public QueenCastle(Piece piece, Rook rook, Game game) : base(piece, null, piece.Color == White ? C1 : C8, game)
        {
            this.rook = rook;
        }

        public override void MakeMove()
        {
            game.MovePiece(Piece, TargetPos, AttackedPiece);
            game.MovePiece(rook, rook.Position + new Vector2(3, 0));
            game.CalculateAvailibleMoves(Piece.Color == White ? Black : White);
        }
    }
}
