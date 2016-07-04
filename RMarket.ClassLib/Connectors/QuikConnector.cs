using DDEInfo;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Connectors
{
    public class QuikConnector: IDataProvider
    {

        private InfoServer _server;

        #region PARAMS

        [Parameter(Description ="Имя DDE сервера")]
        string ServerName { get; set; }

        [Parameter(Description = "Колонка в таблице: Дата")]
        string Col_Date { get; set; }
        [Parameter(Description = "Формат даты")]
        string FormatDate { get; set; }
        [Parameter(Description = "Колонка в таблице: Время")]
        string Col_Time { get; set; }
        [Parameter(Description = "Формат времени")]
        string FormatTime { get; set; }
        [Parameter(Description = "Колонка в таблице: Код бумаги")]
        string Col_TickerCode { get; set; }
        [Parameter(Description = "Колонка в таблице: Цена")]
        string Col_Price { get; set; }
        [Parameter(Description = "Колонка в таблице: Кол-во")]
        string Col_Qty { get; set; }
        [Parameter(Description = "Колонка в таблице: Период")]
        string Col_TradePeriod { get; set; }
        [Parameter(Description = "Значение периода: Открытие")]
        string Val_PeriodOpening { get; set; }
        [Parameter(Description = "Значение периода: Нормальный")]
        string Val_PeriodTrading { get; set; }
        [Parameter(Description = "Значение периода: Закрытие")]
        string Val_PeriodClosing { get; set; }
        [Parameter(Description = "Время начала сессии(если нет колонки период)")]
        TimeSpan Val_SessionStart { get; set; }
        [Parameter(Description = "Время окнчания сессии(если нет колонки период)")]
        TimeSpan Val_SessionFinish { get; set; }


        #endregion

        private Dictionary<string, int> headTable;

        public event EventHandler<TickEventArgs> TickPoked;

        public InfoServer Server { get
            {
                if (_server == null)
                    _server = new InfoServer(ServerName);
                return _server;
            }
            set
            {
                _server = value;
            }
        }

        public QuikConnector() 
        {
            ServerName = "RMarket";
            Col_Date = "Date";
            FormatDate = "yyyyMMdd";
            Col_Time = "Time";
            FormatTime = "HHmmss";
            Col_TickerCode = "TICKER";
            Col_Price = "LAST";
            Col_Qty = "Qty";
            Col_TradePeriod = "Period";
            Val_PeriodOpening = "Opening";
            Val_PeriodTrading = "Trading";
            Val_PeriodClosing = "Closing";
            Val_SessionStart = new TimeSpan(10, 0, 0);
            Val_SessionFinish = new TimeSpan(19, 0, 0);

        }

        public void StartServer()
        {
            Server.DataPoked += OnDataPoked; //!!!Проконтролировать

            if (Server.State!=DDEInfo.eServerState.Registered)
                Server.Register();
            return;
        }
        public void StopServer()
        {
            Server.Unregister();
            return;
        }

        /// <summary>
        /// Метод выполняется по подписке при передачи данных из терминала Quik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataPoked(object sender, DataPokeddEventArgs e)
        {
            bool isRealTime = e.Cells.Length == 1 ? true : false; //Если пришла одна строка, то скорее всего это реал тайм )). Или проверить время сервера

            ConnectorHelper helper = new ConnectorHelper();

            foreach (object[] cells in e.Cells)
            {
                if (headTable == null) //Должна быть строка с заголовками
                {
                    headTable = helper.FillHeadTable(cells); 
                    continue;
                }

                try
                {
                    TickEventArgs tick = new TickEventArgs();
                    tick.Date = helper.ParseDate(cells, headTable, Col_Date, Col_Time, FormatDate, FormatTime);
                    tick.TickerCode = helper.ParseTickerCode(cells, headTable, Col_TickerCode);
                    tick.Price = helper.ParsePrice(cells, headTable, Col_Price, CultureInfo.InvariantCulture);
                    tick.Quantity = helper.ParseQuantity(cells, headTable, Col_Qty);
                    tick.IsRealTime = isRealTime;

                    tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, Col_TradePeriod, Val_PeriodOpening, Val_PeriodTrading, Val_PeriodClosing);
                    if (tick.TradePeriod == TradePeriodEnum.Undefended)
                        tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, Col_TradePeriod, FormatTime, Val_SessionStart, Val_SessionFinish);

                    tick.Extended = helper.CreateExtended(cells, headTable);

                    //вызвать событие IConnector
                    var tickPoked = TickPoked;
                    tickPoked?.Invoke(this, tick);

                    //Запишем тик в историю
                    //!!!TickHelper.InsertNewTick(tick);
                }
                catch(Exception)
                {
                    throw;
                }
            }

        }


    }
}
