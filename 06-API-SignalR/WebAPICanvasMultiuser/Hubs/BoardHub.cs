using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace WebAPICanvasMultiuser.Hubs
{
    public class BoardHub: Hub
    {
        public async Task DrawMessage(double lastX, double lastY,
                                    double offsetX, double offsetY)
        {
            await Clients.All.SendAsync("ReceiveDrawMessage", 
                                        lastX, lastY, 
                                        offsetX, offsetY);
        }
    }
}

