using Chess.Logic.Moves;
using static Chess.Logic.PieceColor;

namespace Chess.Logic.Pieces
{
    internal class Pawn : Piece
    {
        private readonly int startPosY;
        private readonly int promotionY;
        private readonly int enPassantY;
        private readonly Vector2 oneStepUp;
        private readonly Vector2 twoStepsUp;
        private readonly Vector2 rightAttack;
        private readonly Vector2 leftAttack;

        public Pawn(PieceColor color, Vector2 position, int id, Board board) : base(color, position, id, board)
        {
            startPosY = Color == White ? 1 : 6;
            promotionY = Color == White ? 7 : 0;
            enPassantY = Color == White ? 4 : 3;
            oneStepUp = Color == White ? new Vector2(0, 1) : new Vector2(0, -1);
            twoStepsUp = oneStepUp * 2;
            rightAttack = Color == White ? new Vector2(1, 1) : new Vector2(1, -1);
            leftAttack = Color == White ? new Vector2(-1, 1) : new Vector2(-1, -1);
        }

        public override void CalculateMoves()
        {
            base.CalculateMoves();

            if (board.CanMove(Position + oneStepUp))
            {
                AddMoveOrPromotion(Position + oneStepUp, null);
            }

            if (Position.Y == startPosY && board.CanMove(Position + twoStepsUp))
            {
                AddMoveOrPromotion(Position + twoStepsUp, null);
            }

            var deltas = new[] { rightAttack, leftAttack };

            foreach (var delta in deltas)
            {
                if (!(Position + delta).IsValidChessPos())
                    continue;

                PossibleAttacks.Add(Position + delta);

                if (board.CanBeat(Position + delta, Color, out var attackedPiece))
                {
                    AddMoveOrPromotion(Position + delta, attackedPiece);
                    if (attackedPiece.GetType() == typeof(King))
                    {
                        KingAttacks.Add(Position);
                        KingAttacks.Add(Position + delta);
                    }
                }
                else
                {
                    if (board.Moves.Count == 0)
                        continue;

                    var lastMove = board.Moves.Last();

                    if (Position.Y == enPassantY
                        && lastMove.Piece.GetType() == typeof(Pawn)
                        && lastMove.TargetPos.X == Position.X + delta.X
                        && Math.Abs(lastMove.SourcePos.Y - lastMove.TargetPos.Y) == 2)
                    {
                        AddMoveOrPromotion(Position + delta, lastMove.Piece);
                    }
                }
            }
        }

        private void AddMoveOrPromotion(Vector2 targetPos, Piece attackedPiece)
        {
            if (targetPos.Y == promotionY)
            {
                PossibleMoves.Add(new Promotion<Knight>(this, attackedPiece, targetPos, board));
                PossibleMoves.Add(new Promotion<Bishop>(this, attackedPiece, targetPos, board));
                PossibleMoves.Add(new Promotion<Rook>(this, attackedPiece, targetPos, board));
                PossibleMoves.Add(new Promotion<Queen>(this, attackedPiece, targetPos, board));
            }
            else
            {
                PossibleMoves.Add(new Move(this, attackedPiece, targetPos, board));
            }
        }
    }
}
