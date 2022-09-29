using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    internal class Move
    {
        public int Number { get; }
        public Piece Piece { get; }
        public PositionEnum TargetPos { get; }

        public Move(int number, Piece piece, PositionEnum targetPos)
        {
            Number = number;
            Piece = piece;
            TargetPos = targetPos;
        }
    }
}
