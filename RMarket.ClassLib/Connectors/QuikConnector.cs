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

        [Parameter(Description = "Колонка в таблице: Время")]
        string col_Time = "Time";

        [Parameter(Description = "Колонка в таблице: Код бумаги")]
        string col_TickerCode = "Security code";

        [Parameter(Description = "Колонка в таблице: Цена")]
        string col_Price = "Price";

        [Parameter(Description = "Колонка в таблице: Кол-во")]
        string col_Qty = "Qty";

        [Parameter(Description = "Колонка в таблице: Период")]
        string col_Period = "Period";

        #endregion

        private Dictionary<string, int> headTable = new Dictionary<string, int>();

        public event EventHandler<TickEventArgs> TickPoked;
        public bool ServerIsStarted
        {
            get { return Server.IsRegistered; }
        }

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

        #region IConnector

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
        #endregion

        /// <summary>
        /// Метод выполняется по подписке при передачи данных из терминала Quik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataPoked(object sender, DataPokeddEventArgs e)
        {
            bool isRealTime = e.Cells.Length == 1 ? true : false; //Если пришла одна строка, то скорее всего это реал тайм )).

            foreach (object[] cells in e.Cells)
            {
                if (headTable.Count == 0) //Должна быть строка с заголовками
                {
                    FillHeadTable(cells); 
                    continue;
                }

                try
                {
                    TickEventArgs tick = new TickEventArgs();
                    tick.Date = headTable.ContainsKey(col_Date) ? DateTime.ParseExact(cells[headTable[col_Date]].ToString() + cells[headTable[col_Time]].ToString(), "dd.MM.yyyyHH:mm:ss", null) :
                                                                DateTime.ParseExact(cells[headTable[col_Time]].ToString(), "HH:mm:ss", null);
                    tick.TickerCode = cells[headTable[col_TickerCode]].ToString();
                    tick.Price = Convert.ToDecimal(cells[headTable[col_Price]], CultureInfo.InvariantCulture);
                    tick.Quantity = Convert.ToInt32(cells[headTable[col_Qty]]);
                    tick.IsRealTime = isRealTime;

                    if (headTable.ContainsKey(col_Period))
                    {
                        if (cells[headTable[col_Period]].ToString() == "Opening" || cells[headTable[col_Period]].ToString() == "Открытие")
                            tick.TradePeriod = TradePeriodEnum.Opening;
                        else if (cells[headTable[col_Period]].ToString() == "Trading" || cells[headTable[col_Period]].ToString() == "Нормальный")
                            tick.TradePeriod = TradePeriodEnum.Trading;
                        else if (cells[headTable[col_Period]].ToString() == "Closing" || cells[headTable[col_Period]].ToString() == "Закрытие")
                            tick.TradePeriod = TradePeriodEnum.Closing;
                    }

                    foreach (var row in headTable)
                    {
                        tick.Extended.Add(row.Key, cells[row.Value].ToString());
                    }

                    //вызвать событие IConnector
                    if (TickPoked != null)
                        TickPoked(this, tick);

                    //Запишем тик в историю
                    //!!!TickHelper.InsertNewTick(tick);
                }
                catch(Exception)
                { }
            }

        }

        private void FillHeadTable(object[] cells)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                string cell = cells[i] as string;
                headTable.Add(cell, i);
            }

            //!!!проверка на наличие всех обязательных колонок

        }

    }
}
