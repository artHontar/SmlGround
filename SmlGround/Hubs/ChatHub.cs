using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SmlGround.Hubs
{
    public class ChatHub : Hub
    {
        
        public void Send(string text, string date)
        {
            Clients.Others.addMessage(text,date);
        }
    }
}