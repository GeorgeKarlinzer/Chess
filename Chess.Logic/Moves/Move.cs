using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic.Moves
{
    public abstract class Move
    {
        public Vector2 TargetPos { get; }
        public Vector2 SourcePos { get; }
        public Piece Piece { get; }

    }
}
