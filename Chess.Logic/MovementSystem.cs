using static Chess.Logic.PieceType;

namespace Chess.Logic
{
    internal class MovementSystem
    {
        public List<PositionEnum> GetMoves(Piece piece)
        {
            var moves = new List<PositionEnum>();

            switch (piece.Type)
            {
                case Pawn:
                    if (piece.Color == PieceColor.White)
                        AddWhitePawnMoves(moves, piece.Position);
                    else
                        AddBlackPawnMoves(moves, piece.Position);
                    break;
                case Bishop: AddBishopMoves(moves, piece.Position); break;
                case Knight: AddKnightMoves(moves, piece.Position); break;
                case Rook: AddRookMoves(moves, piece.Position); break;
                case Queen: AddQueenMoves(moves, piece.Position); break;
                case King: AddKingMoves(moves, piece.Position); break;
            }

            return moves;
        }

        private void AddWhitePawnMoves(List<PositionEnum> moves, PositionEnum pos)
        {
           AddIfValid(moves, pos + 1);
           AddIfValid(moves, pos + 2);
           AddIfValid(moves, pos + 9);
           AddIfValid(moves, pos - 7);
        }

        private void AddBlackPawnMoves(List<PositionEnum> moves, PositionEnum pos)
        {
            AddIfValid(moves, pos - 1);
            AddIfValid(moves, pos - 2);
            AddIfValid(moves, pos - 9);
            AddIfValid(moves, pos + 7);
        }

        private void AddBishopMoves(List<PositionEnum> moves, PositionEnum pos)
        {
            for (int i = 1; i <= 7; i++)
            {
                AddIfValid(moves, pos + 9 * i);
                AddIfValid(moves, pos - 9 * i);
                AddIfValid(moves, pos + 7 * i);
                AddIfValid(moves, pos - 9 * i);
            }
        }

        private void AddKnightMoves(List<PositionEnum> moves, PositionEnum pos)
        {
            AddIfValid(moves, pos + 10);
            AddIfValid(moves, pos + 17);
            AddIfValid(moves, pos + 15);
            AddIfValid(moves, pos + 6);
            AddIfValid(moves, pos - 10);
            AddIfValid(moves, pos - 17);
            AddIfValid(moves, pos - 15);
            AddIfValid(moves, pos - 6);
        }

        private void AddRookMoves(List<PositionEnum> moves, PositionEnum pos)
        {
            for (int i = 0; i < 7; i++)
            {
                AddIfValid(moves, pos + i);
                AddIfValid(moves, pos - i);
                AddIfValid(moves, pos + i * 8);
                AddIfValid(moves, pos - i * 8);
            }
        }

        private void AddQueenMoves(List<PositionEnum> moves, PositionEnum pos)
        {
            AddBishopMoves(moves, pos);
            AddRookMoves(moves, pos);
        }

        private void AddKingMoves(List<PositionEnum> moves, PositionEnum pos)
        {
            AddIfValid(moves, pos + 1);
            AddIfValid(moves, pos + 9);
            AddIfValid(moves, pos + 8);
            AddIfValid(moves, pos + 7);
            AddIfValid(moves, pos - 1);
            AddIfValid(moves, pos - 9);
            AddIfValid(moves, pos - 8);
            AddIfValid(moves, pos - 7);

            AddIfValid(moves, pos + 16);
            AddIfValid(moves, pos - 16);
        }

        private void AddIfValid(List<PositionEnum> moves, PositionEnum pos)
        {
            if (pos.IsValid())
                moves.Add(pos);
        }
    }
}
