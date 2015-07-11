using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Quince.Utils.Messages;
using Quince.Admin.Core.Managers;

namespace Quince.Admin.Hubs
{
    public class ApiClient : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public Response UpdateETenders(int onPage, int requests)
        {
            var response = new Response();
            foreach (var progress in ApiManager.UpdateETenders(onPage, requests))
            {
                Clients.Caller.updateProgress(progress);
            }
            return response;
        }
        public Response UpdateETenderContracts(int onPage, int requests)
        {
            var response = new Response();
            foreach (var progress in ApiManager.UpdateETenderContracts(onPage, requests))
            {
                Clients.Caller.updateProgress(progress);
            }
            return response;
        }
    }
}