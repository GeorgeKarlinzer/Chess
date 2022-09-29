using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Logic.PieceType;
using static Chess.Logic.PieceColor;
using static Chess.Logic.PositionEnum;

namespace Chess.Logic
{
    public class Board
    {
        private readonly Dictionary<PositionEnum, Piece> piecesMap;

        private readonly Dictionary<Piece, List<PositionEnum>> availibleMovesMap;

        public Board()
        {
            piecesMap = new();
            availibleMovesMap = new();

            GeneratePieces();
        }
        
        private void GeneratePieces()
        {
            for (var pos = a2; pos < h8; pos += 8)
            {
                AddPiece(Pawn, White, pos);
                AddPiece(Pawn, Black, pos + 5);
            }

            AddPiece(Rook, White, a1);
            AddPiece(Rook, White, h1);
            AddPiece(Rook, Black, a8);
            AddPiece(Rook, Black, h8);

            AddPiece(Knight, White, b1);
            AddPiece(Knight, White, g1);
            AddPiece(Knight, Black, b8);
            AddPiece(Knight, Black, g8);

            AddPiece(Bishop, White, c1);
            AddPiece(Knight, White, f1);
            AddPiece(Knight, Black, c8);
            AddPiece(Knight, Black, f8);

            AddPiece(Queen, White, d1);
            AddPiece(Queen, Black, d8);

            AddPiece(King, White, e1);
            AddPiece(King, Black, e8);
        }

        private void AddPiece(PieceType type, PieceColor color, PositionEnum position)
        {
            var piece = new Piece(type, color, position, piecesMap.Count);
            piecesMap[piece.Position] = piece;
        }

        private List<PositionEnum> CalculateAvailibleMoves(Piece piece)
        {
            if (availibleMovesMap.TryGetValue(piece, out var moves))
                return moves;

            var movementSystem = new MovementSystem();

            var moves = movementSystem.GetMoves(piece);

            return null;
        }

        private void MakeMove(Move move)
        {

            availibleMovesMap.Clear();
        }
            
        private bool IsValidMove(Move move)
        {
            return true;
        }
    }
}
