using Chess.Logic.Moves;
using static Chess.Logic.PieceColor;

namespace Chess.Logic.Pieces
{
    internal class Pawn : Piece
    {
        public Pawn(PieceColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            base.CalculateMoves();

            var startPosY = Color == White ? 1 : 6;
            var enPassantY = Color == White ? 4 : 3;
            var oneStepUp = Color == White ? new Vector2(0, 1) : new Vector2(0, -1);
            var twoStepsUp = oneStepUp * 2;
            var rightAttack = Color == White ? new Vector2(1, 1) : new Vector2(1, -1);
            var leftAttack = Color == White ? new Vector2(-1, 1) : new Vector2(-1, -1);

            if (game.CanMove(Position + oneStepUp))
            {
                var move = new Move(this, null, Position + oneStepUp, game);
                PossibleMoves.Add(move);
            }

            if (Position.Y == startPosY && game.CanMove(Position + twoStepsUp))
            {
                var move = new Move(this, null, Position + twoStepsUp, game);
                PossibleMoves.Add(move);
            }

            var deltas = new[] { rightAttack, leftAttack };

            foreach (var delta in deltas)
            {
                if (!(Position + delta).IsValidChessPos())
                    continue;

                PossibleAttacks.Add(Position + delta);

                if (game.CanBeat(Position + delta, Color, out var attackedPiece))
                {
                    var move = new Move(this, attackedPiece, Position + delta, game);
                    PossibleMoves.Add(move);
                    if (attackedPiece.GetType() == typeof(King))
                    {
                        KingAttacks.Add(Position);
                        KingAttacks.Add(Position + delta);
                    }
                }
                else
                {
                    if (game.Moves.Count == 0)
                        continue;

                    var lastMove = game.Moves.Last();

                    if (Position.Y == enPassantY
                        && lastMove.Piece.GetType() == typeof(Pawn)
                        && lastMove.TargetPos.X == Position.X + delta.X
                        && Math.Abs(lastMove.SourcePos.Y - lastMove.TargetPos.Y) == 2)
                    {
                        var move = new Move(this, lastMove.Piece, Position + delta, game);
                        PossibleMoves.Add(move);
                    }
                }
            }
        }
    }
}
