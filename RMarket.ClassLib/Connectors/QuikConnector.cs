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
        string serverName = "RMarket";

        [Parameter(Description = "Колонка в таблице: Дата")]
        string col_Date = "Date";

        [Parameter(Description = "Формат даты")]
        string formatDate = "dd.MM.yyyy";

        [Parameter(Description = "Колонка в таблице: Время")]
        string col_Time = "Time";

        [Parameter(Description = "Формат времени")]
        string formatTime = "HH:mm:ss";

        [Parameter(Description = "Колонка в таблице: Код бумаги")]
        string col_TickerCode = "Security code";

        [Parameter(Description = "Колонка в таблице: Цена")]
        string col_Price = "Price";

        [Parameter(Description = "Колонка в таблице: Кол-во")]
        string col_Qty = "Qty";

        [Parameter(Description = "Колонка в таблице: Период")]
        string col_TradePeriod = "Period";

        [Parameter(Description = "Значение периода: Открытие")]
        string val_PeriodOpening = "Opening";

        [Parameter(Description = "Значение периода: Нормальный")]
        string val_PeriodTrading = "Trading";

        [Parameter(Description = "Значение периода: Закрытие")]
        string val_PeriodClosing = "Closing";

        [Parameter(Description = "Время начала сессии(если нетколонки период)")]
        TimeSpan val_SessionStart = new TimeSpan(10,0,0);

        [Parameter(Description = "Время окнчания сессии(если нетколонки период)")]
        TimeSpan val_SessionFinish = new TimeSpan(19, 0, 0);


        #endregion

        private Dictionary<string, int> headTable;

        public event EventHandler<TickEventArgs> TickPoked;

        public InfoServer Server { get
            {
                if (_server == null)
                    _server = new InfoServer(serverName);
                return _server;
            }
            set
            {
                _server = value;
            }
        }

        public QuikConnector() 
        {
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
                    tick.Date = helper.ParseDate(cells, headTable, col_Date, col_Time, formatDate, formatTime);
                    tick.TickerCode = helper.ParseTickerCode(cells, headTable, col_TickerCode);
                    tick.Price = helper.ParsePrice(cells, headTable, col_Price, CultureInfo.InvariantCulture);
                    tick.Quantity = helper.ParseQuantity(cells, headTable, col_Qty);
                    tick.IsRealTime = isRealTime;

                    tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, col_TradePeriod, val_PeriodOpening, val_PeriodTrading, val_PeriodClosing);
                    if (tick.TradePeriod == TradePeriodEnum.Undefended)
                        tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, col_TradePeriod, formatTime, val_SessionStart, val_SessionFinish);

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
