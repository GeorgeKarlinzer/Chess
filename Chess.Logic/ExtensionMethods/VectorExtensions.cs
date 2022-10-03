using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    public static class VectorExtensions
    {
        public static string ToChessCoord(this Vector2 point)
        {
            var chr = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            return $"{chr[point.X]}{point.Y + 1}";
        }

        public static bool IsValidChessPos(this Vector2 pos) =>
            pos.X < 8 && pos.X >= 0 && pos.Y < 8 && pos.Y >= 0;

        public static Vector2 ToChessPos(this int i) =>
            new (i / 8, i % 8);
    }
}
