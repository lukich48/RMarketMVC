using Microsoft.AspNet.SignalR;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure
{
    public class NotificationHelper:IDisposable
    {
        public string ConnectionId { get; set; }
        /// <summary>
        /// для подписки
        /// </summary>
        private Instrument instr;

        public NotificationHelper(Instrument instr)
        {
            this.instr = instr;

            instr.CreatedCandleReal += OnCreatedCandle;
        }

        protected void OnCreatedCandle(object sender, EventArgs e)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.Client(ConnectionId).candleCreated(1);
            //context.Clients.Group(GetGroupName(tickerCode, timeFrame)).candleCreated(1);

        }

        public void Dispose()
        {
            instr.CreatedCandleReal -= OnCreatedCandle;
        }
    }
}