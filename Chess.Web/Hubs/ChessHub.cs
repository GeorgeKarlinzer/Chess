using Chess.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chess.Web.Hubs
{
    [Authorize]
    public class ChessHub : Hub<IChessClient>
    {
    }
}
