using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    public class Move
    {
        public int Id { get; }
        public Piece Piece { get; }
        public PositionEnum SourcePos { get; }
        public PositionEnum TargetPos { get; }

        public Move(int id, Piece piece, PositionEnum sourcePos, PositionEnum targetPos)
        {
            Id = id;
            Piece = piece;
            TargetPos = targetPos;
            SourcePos = sourcePos;
        }
    }
}
