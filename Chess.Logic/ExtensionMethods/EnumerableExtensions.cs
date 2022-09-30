using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    internal static class EnumerableExtensions
    {
        public static void Foreach<T>(this IEnumerable<T> coll, Action<T> action)
        {
            foreach (var item in coll)
            {
                action(item);
            }
        }
    }
}
