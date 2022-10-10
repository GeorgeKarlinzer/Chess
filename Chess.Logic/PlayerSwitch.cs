using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    internal class PlayerSwitch : IPlayerSwitch
    {
        public PlayerColor Switch(PlayerColor color) =>
            color.IsWhite() ? PlayerColor.Black : PlayerColor.White;
    }
}
