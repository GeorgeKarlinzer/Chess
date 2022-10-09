namespace Chess.Logic
{
    internal static class Positions
    {
        public readonly static IReadOnlyDictionary<string, Vector2> VectorsMap;
        public readonly static IReadOnlyDictionary<Vector2, string> PositionsMap;

        static Positions()
        {
            var vectorsMap = new Dictionary<string, Vector2>();
            var positionsMap = new Dictionary<Vector2, string>();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var v = new Vector2(x, y);
                    var p = $"{(char)(x + 'a')}{y + 1}";
                    positionsMap[v] = p;
                    vectorsMap[p] = v;
                }
            }

            PositionsMap = positionsMap;
            VectorsMap = vectorsMap;
        }

        public static IEnumerable<Vector2> GetVectors(params string[] strings)
        {
            foreach (var str in strings)
                yield return VectorsMap[str];
        }

        public static bool IsValidChessPos(this string pos) => 
            VectorsMap.ContainsKey(pos);

        public static bool IsValidChessPos(this Vector2 pos) =>
            PositionsMap.ContainsKey(pos);
    }
}
