using Chess.Logic.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    public class Cell
    {
        public Vector2 Coordinates { get; }

        public Piece Piece { get; set; }

        public Cell(Vector2 coordinates)
        {
            Coordinates = coordinates;
        }

        public Cell(int x, int y) : this(new Vector2(x, y))
        {
        }
    }
}
