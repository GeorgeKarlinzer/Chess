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

        public static int GetX(this PositionEnum position) =>
            position.ToTuple().x;

        public static int GetY(this PositionEnum position) =>
            position.ToTuple().y;

        public static bool IsValid(this PositionEnum position) =>
            position >= a1 && position <= h8;

        public static bool IsSameLine(this PositionEnum pos1, PositionEnum pos2)
        {
            var (x1, y1) = pos1.ToTuple();
            var (x2, y2) = pos2.ToTuple();

            return x1 == x2 || y1 == y2 || x1 - x2 - y1 + y2 == 0;
        }

        public static bool IsBetween(this PositionEnum pos, PositionEnum pos1, PositionEnum pos2)
        {
            if (!pos1.IsSameLine(pos2) || pos.IsSameLine(pos1))
                return false;

            var (x, y) = pos.ToTuple();
            var (x1, y1) = pos1.ToTuple();
            var (x2, y2) = pos2.ToTuple();

            var v1 = (Math.Sign(x1 - x), Math.Sign(y1 - y));
            var v2 = (Math.Sign(x2 - x), Math.Sign(y2 - y));

            return v1 == v2;
        }
    }
}
