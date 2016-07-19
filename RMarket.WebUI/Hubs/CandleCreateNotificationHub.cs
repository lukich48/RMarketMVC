using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using RMarket.WebUI.Infrastructure;

namespace RMarket.WebUI.Hubs
{
    public class CandleCreateNotificationHub : Hub
    {
        private static List<NotificationHelper> notificationHelpers= new List<NotificationHelper>();

        /// <summary>
        /// Подключение пользователя
        /// </summary>
        /// <param name="tickerCode"></param>
        /// <param name="timeFrame"></param>
        public void Connect(int resultId)
        {
            NotificationHelper helper = new NotificationHelper
            {
                ClientId = Context.ConnectionId
            };

            //подписываемся нв событие формирования свечи


            //Groups.Add(Context.ConnectionId, GetGroupName(tickerCode, timeFrame));
        }

        /// <summary>
        /// Оповестим клиентов о сформерованой свече
        /// </summary>
        /// <param name="tickerCode"></param>
        /// <param name="timeFrame"></param>
        public static void CreatedCandleSend(string tickerCode, string timeFrame)
        {
            //Пока статический   
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CandleCreateNotificationHub>();
            context.Clients.Group(GetGroupName(tickerCode, timeFrame)).candleCreated(1);
        }

        private static string GetGroupName(string tickerCode, string timeFrame)
        {
            return tickerCode + "_" + timeFrame;
        }
    }
}