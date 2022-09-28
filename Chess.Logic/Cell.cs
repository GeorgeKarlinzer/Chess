using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    internal class Cell
    {
        public PositionEnum Position { get; }

        public Piece Piece { get; set; }

        public Cell(PositionEnum position)
        {
            Position = position;
        }
    }
}
