using System.Runtime.InteropServices;

namespace Chess.Logic
{
    internal class Clock
    {
        private Dictionary<PlayerColor, int> remainTimeMap;
        private DateTime lastTimeStamp;

        private IPlayerSwitch playerSwitch;
        private PlayerColor currentPlayer;
        public bool IsStoped { get; private set; }
        public bool IsPaused { get; private set; }

        private readonly int bonus;

        private int CurrentTime
        {
            get => remainTimeMap[currentPlayer];
            set => remainTimeMap[currentPlayer] = value;
        }

        public Clock(int timeSec, int bonusSec, IPlayerSwitch playerSwitch)
        {
            remainTimeMap = new()
            {
                [PlayerColor.White] = timeSec * 1000,
                [PlayerColor.Black] = timeSec * 1000
            };
            currentPlayer = PlayerColor.White;
            IsStoped = false;
            IsPaused = true;

            bonus = bonusSec * 1000;
            this.playerSwitch = playerSwitch;
        }

        public int GetTime(PlayerColor player) =>
            remainTimeMap[player];

        public Dictionary<PlayerColor, int> GetRemainTimes() =>
            new(remainTimeMap);

        public void PressAndPause()
        {
            if (!IsPaused && !IsStoped)
            {
                var deltaTime = (int)(DateTime.UtcNow - lastTimeStamp).TotalMilliseconds;
                CurrentTime -= deltaTime - bonus;

                if(CurrentTime <= 0)
                {
                    CurrentTime = 0;
                    IsStoped = true;
                }
            }
        }

        public void StartForNextPlayer()
        {
            if (IsStoped) return;

            IsPaused = false;
            lastTimeStamp = DateTime.UtcNow;
            currentPlayer = playerSwitch.Switch(currentPlayer);
        }

        public void Stop() =>
            IsStoped = true;
    }
}
