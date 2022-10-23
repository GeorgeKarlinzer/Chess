using Chess.Logic.Dtos;

namespace Chess.Logic
{
    internal class Clock
    {
        private readonly Dictionary<PlayerColor, int> remainTimeMap;
        private readonly IPlayerSwitch playerSwitch;
        private readonly int bonus;

        private DateTime lastTimeStamp;
        private PlayerColor currentPlayer;
        public bool IsStopped { get; private set; }
        public bool IsPaused { get; private set; }


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
            IsStopped = false;
            IsPaused = true;

            bonus = bonusSec * 1000;
            this.playerSwitch = playerSwitch;
        }

        public int GetTime(PlayerColor player) =>
            remainTimeMap[player];

        public Dictionary<PlayerColor, TimerDto> GetTimersMap()
        {
            var timersMap = new Dictionary<PlayerColor, TimerDto>();

            foreach (var color in playerSwitch.GetColors())
                timersMap[color] = new(remainTimeMap[color], currentPlayer == color && !IsStopped);

            return timersMap;
        }

        public void PressAndPause()
        {
            if (!IsPaused && !IsStopped)
            {
                var deltaTime = (int)(DateTime.UtcNow - lastTimeStamp).TotalMilliseconds;
                CurrentTime -= deltaTime - bonus;

                if (CurrentTime <= 0)
                {
                    CurrentTime = 0;
                    IsStopped = true;
                }
                IsPaused = true;
            }
        }

        public void StartForNextPlayer()
        {
            if (IsStopped) return;

            IsPaused = false;
            lastTimeStamp = DateTime.UtcNow;
            currentPlayer = playerSwitch.Switch(currentPlayer);
        }

        public void Stop() =>
            IsStopped = true;
    }
}
