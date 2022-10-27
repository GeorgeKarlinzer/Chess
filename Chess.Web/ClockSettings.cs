namespace Chess.Web
{
    public struct ClockSettings
    {
        public int Time { get; set; }
        public int Bonus { get; set; }

        public static bool operator ==(ClockSettings s1, ClockSettings s2) =>
            s1.Time == s2.Time && s1.Bonus == s2.Bonus;

        public static bool operator !=(ClockSettings s1, ClockSettings s2) =>
            !(s1 == s2);

        public override bool Equals(object? obj) =>
            obj is ClockSettings cs && cs == this;

        public override int GetHashCode() =>
            Time.GetHashCode() ^ 128921332 + Bonus.GetHashCode() ^ 998318984;
    }
}
