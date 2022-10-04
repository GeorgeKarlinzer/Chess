using Chess.Logic.Moves;
using static Chess.Logic.PieceColor;

namespace Chess.Logic.Pieces
{
    public abstract class Piece
    {
        protected Game game;

        public int Id { get; }
        public PieceColor Color { get; }
        public Vector2 Position { get; set; }

        public List<Move> PossibleMoves { get; }
        public List<Vector2> PossibleAttacks { get; }

        public Piece(PieceColor color, Vector2 position, int id, Game game)
        {
            Color = color;
            Position = position;
            Id = id;
            this.game = game;

            PossibleMoves = new List<Move>();
            PossibleAttacks = new List<Vector2>();
        }

        public abstract void CalculateMoves();

        protected void AddContinuousMove(Vector2 deltaPos)
        {
            var targetPos = Position;

            while ((targetPos += deltaPos).IsValidChessPos())
            {
                PossibleAttacks.Add(deltaPos);

                if (game.CanMove(targetPos))
                {
                    PossibleMoves.Add(CreateMove(targetPos));
                }
                else
                {
                    if (game.CanBeat(targetPos, Color, out var attackedPiece))
                        PossibleMoves.Add(CreateMove(targetPos, attackedPiece));

                    return;
                }
            }
        }

        protected Move CreateMove(Vector2 targetPos, Piece attackedPiece = null) =>
            new(this, attackedPiece, targetPos, game);
    }

    public class Pawn : Piece
    {
        public Pawn(PieceColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            PossibleMoves.Clear();
            PossibleAttacks.Clear();

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
                if(delta.IsValidChessPos())
                    PossibleAttacks.Add(Position + delta);

                if (game.CanBeat(Position + delta, Color, out var attackedPiece))
                {
                    var move = new Move(this, attackedPiece, Position + delta, game);
                    PossibleMoves.Add(move);
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

    public class Bishop : Piece
    {
        public Bishop(PieceColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            PossibleMoves.Clear();
            PossibleAttacks.Clear();

            AddContinuousMove(new(1, 1));
            AddContinuousMove(new(-1, -1));
            AddContinuousMove(new(1, -1));
            AddContinuousMove(new(-1, 1));
        }
    }

    public class Knight : Piece
    {
        public Knight(PieceColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            PossibleMoves.Clear();
            PossibleAttacks.Clear();

            Vector2[] deltaPositions = new Vector2[]
            {
                new(1, 2), new(-1, 2), new(2, 1), new(-2, 1),
                new(1, -2), new(-1, -2), new(2, -1), new(-2, -1)
            };

            foreach (var deltaPos in deltaPositions)
            {
                var pos = Position + deltaPos;

                if (pos.IsValidChessPos())
                    PossibleAttacks.Add(pos);

                if (game.CanBeat(pos, Color, out var attackedPiece) || game.CanMove(pos))
                {
                    PossibleMoves.Add(CreateMove(pos, attackedPiece));
                }
            }
        }
    }

    public class Rook : Piece
    {
        public Rook(PieceColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            PossibleMoves.Clear();
            PossibleAttacks.Clear();

            AddContinuousMove(new(1, 0));
            AddContinuousMove(new(-1, 0));
            AddContinuousMove(new(0, 1));
            AddContinuousMove(new(0, -1));
        }
    }

    public class Queen : Piece
    {
        public Queen(PieceColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            PossibleMoves.Clear();
            PossibleAttacks.Clear();

            AddContinuousMove(new(1, 0));
            AddContinuousMove(new(-1, 0));
            AddContinuousMove(new(0, 1));
            AddContinuousMove(new(0, -1));
            AddContinuousMove(new(1, 1));
            AddContinuousMove(new(-1, -1));
            AddContinuousMove(new(1, -1));
            AddContinuousMove(new(-1, 1));
        }
    }

    public class King : Piece
    {
        public King(PieceColor color, Vector2 position, int id, Game game) : base(color, position, id, game)
        {
        }

        public override void CalculateMoves()
        {
            PossibleMoves.Clear();
            PossibleAttacks.Clear();

            Vector2[] deltaPositions = new Vector2[]
            {
                new(1, 0), new(0, 1), new(1, -1), new(1, 1),
                new(-1, 0), new(0, -1), new(-1, 1), new(-1, -1)
            };

            foreach (var deltaPos in deltaPositions)
            {
                var pos = Position + deltaPos;

                if (pos.IsValidChessPos())
                    PossibleAttacks.Add(pos);

                if (game.CanBeat(pos, Color, out var attackedPiece) || game.CanMove(pos))
                {
                    PossibleMoves.Add(CreateMove(pos, attackedPiece));
                }
            }
        }
    }
}
