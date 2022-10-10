using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    internal static class PieceColorExtensions
    {
        public static bool IsWhite(this PlayerColor color) =>
            color == PlayerColor.White;
    }
}
