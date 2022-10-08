using Chess.Logic;
using Chess.Logic.Pieces;
using Chess.Models;

namespace Chess.Logic.ExtensionMethods
{
    public static class DtoExtensions
    {
        public static PieceDto ToDto(this Piece piece)
        {
            var dto = new PieceDto()
            {
                Id = piece.Id,
                Name = piece.GetType().Name,
                Color = piece.Color.ToString(),
                Position = piece.Position.ToDto(),
                Moves = piece.PossibleMoves.Select(x =>
                    new PositionDto() { X = x.TargetPos.X, Y = x.TargetPos.Y }).ToList()
            };

            return dto;
        }

        public static PositionDto ToDto(this Vector2 pos)
        {
            return new PositionDto() { X = pos.X, Y = pos.Y };
        }
    }
}
