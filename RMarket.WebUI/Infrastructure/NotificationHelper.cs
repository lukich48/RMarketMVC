using Microsoft.AspNet.SignalR;
using RMarket.WebUI.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure
{
    public class NotificationHelper
    {
        public string ConnectionId { get; set; }

        public void OnCreatedCandle(object sender, EventArgs e)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CandleCreateNotificationHub>();
            context.Clients.Client(ConnectionId).candleCreated(1);
            //context.Clients.Group(GetGroupName(tickerCode, timeFrame)).candleCreated(1);

        }
    }
}