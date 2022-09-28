using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    public class Move
    {
        public int Number { get; }
        public Piece Piece { get; }
        public Cell TargetCell { get; }

        public Move(int number, Piece piece, Cell targetCell)
        {
            Number = number;
            Piece = piece;
            TargetCell = targetCell;
        }
    }
}
