using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace InformationHub
{
    public class NotifyHub : Hub
    {
        public void Notify(string name, string message)
        {
            Clients.All.GetNotify( name,  message);
        }
    }
}