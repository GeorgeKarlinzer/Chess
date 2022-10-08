using Chess.Logic.Moves;
using System.Net.NetworkInformation;

namespace Chess.Logic.Pieces
{
    internal abstract class Piece
    {
        protected Game game;

        public int Id { get; }
        public PieceColor Color { get; }
        public Vector2 Position { get; set; }
        public bool IsMoved { get; set; }

        public List<Move> PossibleMoves { get; }
        public List<Vector2> PossibleAttacks { get; }
        public List<Vector2> KingAttacks { get; }

        public Piece(PieceColor color, Vector2 position, int id, Game game)
        {
            Color = color;
            Position = position;
            Id = id;
            this.game = game;

            PossibleMoves = new List<Move>();
            PossibleAttacks = new List<Vector2>();
            KingAttacks = new List<Vector2>();
        }

        public virtual void CalculateMoves()
        {
            PossibleMoves.Clear();
            PossibleAttacks.Clear();
            KingAttacks.Clear();
        }

        protected void AddContinuousMove(Vector2 deltaPos)
        {
            var targetPos = Position;

            var flag = false;
            while ((targetPos += deltaPos).IsValidChessPos())
            {
                if (flag)
                    return;

                PossibleAttacks.Add(targetPos);

                if (game.CanMove(targetPos))
                {
                    PossibleMoves.Add(CreateMove(targetPos));
                }
                else
                {
                    flag = true;
                    if (game.CanBeat(targetPos, Color, out var attackedPiece))
                    {
                        PossibleMoves.Add(CreateMove(targetPos, attackedPiece));
                        if (attackedPiece.GetType() == typeof(King))
                        {
                            var temp = Position - deltaPos;
                            while ((temp += deltaPos) != attackedPiece.Position)
                            {
                                KingAttacks.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        protected Move CreateMove(Vector2 targetPos, Piece attackedPiece = null) =>
            new(this, attackedPiece, targetPos, game);
    }
}
