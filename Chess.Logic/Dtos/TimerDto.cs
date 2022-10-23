using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic.Dtos
{
    public record class TimerDto(int RemainMilliseconds, bool IsRunning);
}
