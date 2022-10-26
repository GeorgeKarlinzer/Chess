namespace Chess.Logic
{
    internal class PlayerSwitch : IPlayerSwitch
    {
        public PlayerColor CurrentPlayer { get; private set; }

        public PlayerSwitch(PlayerColor currentPlayer)
        {
            CurrentPlayer = currentPlayer;
        }

        public void Switch() =>
            CurrentPlayer = (PlayerColor)(1 - (int)CurrentPlayer);

        public IEnumerable<PlayerColor> GetPlayers() =>
            new[] { PlayerColor.White, PlayerColor.Black };
    }
}
