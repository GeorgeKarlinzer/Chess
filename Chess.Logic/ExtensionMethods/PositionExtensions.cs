using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Logic.PositionEnum;

namespace Chess.Logic
{
    internal static class PositionExtensions
    {
        public static (int x, int y) ToTuple(this PositionEnum position)
        {
            var x = (int)position / 8;
            var y = (int)position % 8;

            return (x, y);
        }

        public static bool IsValid(this PositionEnum position) =>
            position >= a1 && position <= h8;
    }
}
