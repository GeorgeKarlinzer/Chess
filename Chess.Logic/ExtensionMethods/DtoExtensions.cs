using Chess.Logic;
using Chess.Logic.Moves;
using Chess.Logic.Pieces;
using Chess.Models;

namespace Chess.Logic
{
    internal static class DtoExtensions
    {
        public static PieceDto ToDto(this Piece piece, PieceColor currentPlayer)
        {
            var dto = new PieceDto()
            {
                Id = piece.Id,
                Name = piece.GetType().Name.ToLower(),
                Color = piece.Color.ToString().ToLower(),
                Position = piece.Position.ToDto(),
                Moves = new List<PositionDto>()
            };

            if (piece.Color == currentPlayer)
                dto.Moves = piece.PossibleMoves.Select(x =>
                    new PositionDto() { X = x.TargetPos.X, Y = x.TargetPos.Y }).ToList();

            return dto;
        }

        public static PositionDto ToDto(this Vector2 pos)
        {
            return new PositionDto() { X = pos.X, Y = pos.Y };
        }

        public static Vector2 ToVector(this PositionDto pos)
        {
            return new Vector2(pos.X, pos.Y);
        }
    }
}
