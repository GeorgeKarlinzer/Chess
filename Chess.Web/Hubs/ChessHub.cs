using Chess.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chess.Web.Hubs
{
    [Authorize]
    public class ChessHub : Hub<IChessClient>
    {
        //private readonly static ConnectionMapping<string> _connections = new();

        //public Task UpdateBoard(GameState gameState, string user)
        //{
        //    var connections = _connections.GetConnections(user);
        //    return Clients.Clients(connections).UpdateBoard(gameState);
        //}


        //public override Task OnConnectedAsync()
        //{
        //    var name = Context.User!.Identity!.Name;

        //    _connections.Add(name!, Context.ConnectionId);
                
        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var name = Context.User!.Identity!.Name;

        //    _connections.Remove(name!, Context.ConnectionId);

        //    return base.OnDisconnectedAsync(exception);
        //}
    }
}
