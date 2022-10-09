namespace Chess.Models
{
    public class PositionDto
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj) =>
            obj is PositionDto other && other == this;

        public static bool operator ==(PositionDto p1, PositionDto p2) =>
            p1.X == p2.X && p1.Y == p2.Y;

        public static bool operator !=(PositionDto p1, PositionDto p2) =>
            !(p1 == p2);

        public override int GetHashCode()
        {
            return 341831928 ^ X.GetHashCode() + -123999123 ^ Y.GetHashCode();
        }
    }
}
