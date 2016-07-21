using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using RMarket.WebUI.Infrastructure;
using RMarket.ClassLib.Models;

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
        public void Connect(int aliveId)
        {
            NotificationHelper helper = new NotificationHelper
            {
                ConnectionId = Context.ConnectionId
            };

            //подписываемся на событие формирования свечи
            AliveResult aliveResult = CurrentUI.AliveResults.FirstOrDefault(t => t.AliveId == aliveId);
            aliveResult.Manager.Instr.CreatedCandleReal += helper.OnCreatedCandle;

        }

        ///// <summary>
        ///// Оповестим клиентов о сформерованой свече
        ///// </summary>
        ///// <param name="tickerCode"></param>
        ///// <param name="timeFrame"></param>
        //public static void CreatedCandleSend(string tickerCode, string timeFrame)
        //{
        //    //Пока статический   
        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CandleCreateNotificationHub>();
        //    context.Clients.Group(GetGroupName(tickerCode, timeFrame)).candleCreated(1);
        //}

    }
}