namespace Chess.Logic
{
    internal class Clock
    {
        private Dictionary<PlayerColor, int> remainTime;
        private DateTime lastTimeStamp;
        private IPlayerSwitch playerSwitch;
        private PlayerColor currentPlayer;

        public Clock(int time, int bonus, IPlayerSwitch playerSwitch)
        {
            remainTime = new()
            {
                [PlayerColor.White] = time * 1000,
                [PlayerColor.Black] = bonus * 1000
            };

            lastTimeStamp = DateTime.MinValue;
            this.playerSwitch = playerSwitch;
        }

        public void SwitchPlayer()
        {
            if(lastTimeStamp != DateTime.MinValue)
            {
            }

            lastTimeStamp = DateTime.UtcNow;
        }

        public void Stop()
        {
            remainTime.Clear();
        }
    }
}
