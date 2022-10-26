using Chess.Logic.Moves;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace Chess.Logic.Pieces
{
    internal abstract class Piece
    {
        public static IReadOnlyDictionary<Type, string> PieceNamesMap = new Dictionary<Type, string>()
        {
            { typeof(Pawn), "pawn" },
            { typeof(Bishop), "bishop" },
            { typeof(Knight), "knight" },
            { typeof(Rook), "rook" },
            { typeof(Queen), "queen" },
            { typeof(King), "king" },

        };

        public static IReadOnlyDictionary<Type, string> PieceCodesMap = new Dictionary<Type, string>()
        {
            { typeof(Pawn), "P" },
            { typeof(Bishop), "B" },
            { typeof(Knight), "N" },
            { typeof(Rook), "R" },
            { typeof(Queen), "Q" },
            { typeof(King), "K" },
        };

        protected Game game;

        public int Id { get; }
        public string Name => PieceNamesMap[GetType()];
        public string Code => PieceCodesMap[GetType()];
        public string FENCode => Color.IsWhite() ? Code : Code.ToLower();
        public PlayerColor Color { get; }
        public Vector2 Position { get; set; }
        public bool IsMoved { get; set; }

        public List<Move> PossibleMoves { get; }
        public List<Vector2> PossibleAttacks { get; }
        public List<Vector2> KingAttacks { get; }

        public Piece(PlayerColor color, Vector2 position, int id, Game game)
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
