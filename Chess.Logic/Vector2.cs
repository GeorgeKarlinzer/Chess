using System.Diagnostics.CodeAnalysis;

namespace Chess.Logic
{
    public struct Vector2
    {
        public int X { get; }
        public int Y { get; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsSameLine(Vector2 v) =>
            X == v.X || Y == v.Y || X - v.X - Y + v.Y == 0;

        public bool IsBetween(Vector2 v1, Vector2 v2)
        {
            if (!v1.IsSameLine(v2) || !IsSameLine(v2))
                return false;

            var a = new Vector2(Math.Sign(v1.X - X), Math.Sign(v1.Y - Y));
            var b = new Vector2(Math.Sign(v2.X - X), Math.Sign(v2.X - Y));

            return a == b;
        }

        public bool IsColliniar(Vector2 v2) =>
            X * v2.Y - Y * v2.X == 0;

        public static Vector2 operator +(Vector2 a, Vector2 b) =>
            new(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator -(Vector2 a, Vector2 b) =>
            new(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, int b) =>
            new(a.X * b, a.Y * b);

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj is not Vector2 v)
                return false;

            return X == v.X && Y == v.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() >> 3 ^ Y.GetHashCode();
        }

        public static bool operator ==(Vector2 a, Vector2 b) =>
            a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Vector2 a, Vector2 b) =>
            !(a == b);

        public override string ToString() =>
            $"{X}:{Y}";
    }
}