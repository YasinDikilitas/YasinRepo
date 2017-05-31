using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace KodlaTv.WebApp
{
    public class ChatHub : Hub
    {
        public void Send(string username, string message,int group)
        {
            Clients.All.sendMessage(username, message,group);
        }
    }
}