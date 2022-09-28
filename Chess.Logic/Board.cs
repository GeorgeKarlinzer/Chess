using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    internal class Board
    {
        private readonly Dictionary<PositionEnum, Cell> cellsMap;
        private readonly List<Piece> pieces;
        private readonly Dictionary<Piece, List<Cell>> availibleMovesMap;

        public Board()
        {
            cellsMap = new();
            pieces = new();
            availibleMovesMap = new();

            GenerateCells();
            GeneratePieces();
        }
        
        private void GeneratePieces()
        {
            var white = PlayerColor.White;
            var black = PlayerColor.Black;

            for (var pos = PositionEnum.a2; (int)pos < 57; pos += 8)
            {
                var whitePiece = new Piece(white, cellsMap[pos]);
                var blackPiece = new Piece(black, cellsMap[pos + 5]);

                pieces.Add(whitePiece);
                pieces.Add(blackPiece);
            }


        }

        private void GenerateCells()
        {
            for (var pos = PositionEnum.a1; pos <= PositionEnum.h8; pos++)
            {
                cellsMap[pos] = new Cell(pos);
            }
        }

        private void MakeMove(Move move)
        {

        }
            
        private bool IsValidMove(Move move)
        {
            return true;
        }
    }
}
