using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RMarket.ClassLib.Helpers;
using System.Globalization;

namespace RMarket.ClassLib.Connectors
{
    public class CsvFileConnector: IDataProvider
    {
        private Dictionary<string, int> headTable = new Dictionary<string, int>();

        #region PARAMS

        [Parameter(Description = "Путь к файлу")]
        string filePath = "";

        [Parameter(Description = "Разделитель")]
        char separator = ';';

        [Parameter(Description = "Колонка в таблице: Дата")]
        string col_Date = "Date";

        [Parameter(Description = "Формат даты")]
        string formatDate = "yyyyMMdd";

        [Parameter(Description = "Колонка в таблице: Время")]
        string col_Time = "Time";

        [Parameter(Description = "Формат времени")]
        string formatTime = "HHmmss";

        [Parameter(Description = "Колонка в таблице: Код бумаги")]
        string col_TickerCode = "TICKER";

        [Parameter(Description = "Колонка в таблице: Цена")]
        string col_Price = "LAST";

        [Parameter(Description = "Колонка в таблице: Кол-во")]
        string col_Qty = "Qty";

        [Parameter(Description = "Объем (если не заполнено количество)")]
        string col_Volume = "VOL";

        [Parameter(Description = "Колонка в таблице: Период")]
        string col_TradePeriod = "Period";

        [Parameter(Description = "Значение периода: Открытие")]
        string val_PeriodOpening = "Opening";

        [Parameter(Description = "Значение периода: Нормальный")]
        string val_PeriodTrading = "Trading";

        [Parameter(Description = "Значение периода: Закрытие")]
        string val_PeriodClosing = "Closing";

        [Parameter(Description = "Время начала сессии(если нет колонки период)")]
        TimeSpan val_SessionStart = new TimeSpan(10, 0, 0);

        [Parameter(Description = "Время окнчания сессии(если нет колонки период)")]
        TimeSpan val_SessionFinish = new TimeSpan(19, 0, 0);


        #endregion

        public event EventHandler<TickEventArgs> TickPoked;

        public void StartServer()
        {
            //запустить обход файла
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] cells = line.Split(separator);

                    bool isRealTime = true;

                    ConnectorHelper helper = new ConnectorHelper();

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
                        if(tick.Quantity==0)
                            tick.Quantity = helper.ParseQuantity(cells, headTable, col_Volume, 1);//!!! заполнить инфо о размере лота

                        tick.IsRealTime = isRealTime;

                        tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, col_TradePeriod, val_PeriodOpening, val_PeriodTrading, val_PeriodClosing);
                        if (tick.TradePeriod == TradePeriodEnum.Undefended)
                            tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, col_TradePeriod, formatTime, val_SessionStart, val_SessionFinish);

                        tick.Extended = helper.CreateExtended(cells, headTable);

                        //вызвать событие IConnector
                        var tickPoked = TickPoked;
                        tickPoked?.Invoke(this, tick);
                        
                    }
                    catch (Exception)
                    {
                        throw;
                    }



                }

            }


        }
        public void StopServer()
        {
            //закрыть файл
        }


    }
}
