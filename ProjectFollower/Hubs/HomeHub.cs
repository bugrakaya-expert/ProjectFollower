using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFollower.Hubs
{
    public class HomeHub : Hub
    {/*
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);//WORKING!!
        }*/
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
