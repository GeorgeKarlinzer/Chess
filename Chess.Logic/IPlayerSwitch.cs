namespace Chess.Logic
{
    internal interface IPlayerSwitch
    {
        PlayerColor Switch(PlayerColor color);
        PlayerColor SwitchBack(PlayerColor color);
    }
}
