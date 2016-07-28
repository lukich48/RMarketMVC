using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using RMarket.WebUI.Infrastructure;
using RMarket.ClassLib.Models;
using System.Threading.Tasks;

namespace RMarket.WebUI.Hubs
{
    public class NotificationHub : Hub
    {
        private static List<NotificationHelper> notificationHelpers= new List<NotificationHelper>();

        /// <summary>
        /// Подключение пользователя
        /// </summary>
        /// <param name="tickerCode"></param>
        /// <param name="timeFrame"></param>
        public void Connect(int aliveId)
        {
 
            //подписываемся на событие формирования свечи
            AliveResult aliveResult = CurrentUI.AliveResults.FirstOrDefault(t => t.AliveId == aliveId);
            if (aliveResult != null)
            {
                NotificationHelper helper = new NotificationHelper(aliveResult.Manager.Instr)
                {
                    ConnectionId = Context.ConnectionId
                };
                notificationHelpers.Add(helper);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            NotificationHelper foundHelper = notificationHelpers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if(foundHelper!=null)
            {
                notificationHelpers.Remove(foundHelper);
                foundHelper.Dispose(); 
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}