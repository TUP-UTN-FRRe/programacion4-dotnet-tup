using Microsoft.AspNetCore.SignalR;

namespace WebAPICanvasMultiuser.Hubs
{
    public class BoardHub: Hub
    {
        public async Task DrawMessage(string x, string y)
        {
            await Clients.All.SendAsync("ReceiveDrawMessage", x, y);
        }
    }
}

