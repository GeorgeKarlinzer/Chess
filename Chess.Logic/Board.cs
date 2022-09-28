using Chess.Logic.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    public class Board
    {
        private readonly Cell[,] cells;
        private readonly List<Piece> pieces;
        private readonly Dictionary<Piece, List<Cell>> availibleMovesMap;

        private int move;

        public Board()
        {
            cells = new Cell[8, 8];
            pieces = new List<Piece>();
            availibleMovesMap = new Dictionary<Piece, List<Cell>>();
            move = 1;

            GenerateSells();
            GeneratePieces();
        }
        
        private void GeneratePieces()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {

                }
            }
        }

        private void GenerateSells()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    cells[x, y] = new Cell(x, y);
                }
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
