using Chess.Logic.Pieces;

namespace Chess.Logic
{
    internal static class DtoExtensions
    {
        public static PieceDto ToDto(this Piece piece, PlayerColor requester, PlayerColor currentPlayer)
        {
            var dto = new PieceDto()
            {
                Id = piece.Id,
                Name = piece.GetType().Name.ToLower(),
                Color = piece.Color,
                Position = piece.Position,
                Moves = new List<Vector2>()
            };

            if (piece.Color == currentPlayer && currentPlayer == requester)
                dto.Moves = piece.PossibleMoves.Select(x => x.TargetPos).ToList();

            return dto;
        }
    }
}
