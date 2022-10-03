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

        public Vector2 ToUnitDirection()
        {
            var a = Math.Max(Math.Abs(X), Math.Abs(Y));
            return this / a;
        }

        public bool IsSameLine(Vector2 v) =>
            X == v.X || Y == v.Y || Math.Abs(X - v.X) == Math.Abs(Y - v.Y);

        public bool IsBetween(Vector2 v1, Vector2 v2)
        {
            if (!IsSameLine(v1) || !IsSameLine(v2) || !v1.IsSameLine(v2))
                return false;

            var a = (v1 - this).ToUnitDirection();
            var b = (this - v2).ToUnitDirection();

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

        public static Vector2 operator /(Vector2 a, int b) =>
            new(a.X / b, a.Y / b);

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