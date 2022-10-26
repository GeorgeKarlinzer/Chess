using Chess.Logic.Dtos;

namespace Chess.Logic
{
    internal class Clock
    {
        private readonly Dictionary<PlayerColor, int> remainTimeMap;
        private readonly IPlayerSwitch playerSwitch;
        private readonly int bonus;

        private int CurrentTime
        {
            get => remainTimeMap[playerSwitch.CurrentPlayer];
            set => remainTimeMap[playerSwitch.CurrentPlayer] = value;
        }
        public bool IsStopped { get; private set; }

        public event Action<PlayerColor> TimedOut;

        public Clock(int timeSec, int bonusSec, IPlayerSwitch playerSwitch)
        {
            this.playerSwitch = playerSwitch;
            IsStopped = false;
            remainTimeMap = new();

            foreach (var player in playerSwitch.GetPlayers())
                remainTimeMap[player] = timeSec * 1000;

            bonus = bonusSec * 1000;
        }

        public Dictionary<PlayerColor, TimerDto> GetTimersMap()
        {
            var timersMap = new Dictionary<PlayerColor, TimerDto>();

            foreach (var player in playerSwitch.GetPlayers())
                timersMap[player] = new(GetTime(player), IsRunningForPlayer(player));

            return timersMap;
        }

        private bool IsRunningForPlayer(PlayerColor player) =>
            playerSwitch.CurrentPlayer == player && !IsStopped;

        public async Task RunTimer()
        {
            while (!IsStopped)
            {
                var timeStamp = DateTime.UtcNow;
                await Task.Delay(10);
                var deltaTime = (int)(DateTime.UtcNow - timeStamp).TotalMilliseconds;
                CurrentTime -= deltaTime - bonus;
                if(CurrentTime <= 0)
                {
                    CurrentTime = 0;
                    IsStopped = true;
                    TimedOut?.Invoke(playerSwitch.CurrentPlayer);
                }
            }
        }

        public int GetTime(PlayerColor color) =>
            remainTimeMap[color];

        public void Stop() =>
            IsStopped = true;
    }
}
