namespace Chess.Logic
{
    internal interface IPlayerSwitch
    {
        PlayerColor CurrentPlayer { get; }
        void Switch();
        IEnumerable<PlayerColor> GetPlayers();
    }
}
