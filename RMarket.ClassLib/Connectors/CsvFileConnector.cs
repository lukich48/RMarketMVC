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
using RMarket.ClassLib.EFRepository;
using RMarket.ClassLib.Entities;
using System.Threading;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.ClassLib.Connectors
{
    public class CsvFileConnector : IDataProvider
    {
        private ITickerRepository tickerRepository;

        private Dictionary<string, int> headTable;
        private CancellationTokenSource cts;

        #region PARAMS

        [Parameter(Description = "Путь к файлу")]
        public string FilePath { get; set; }
        [Parameter(Description = "Разделитель")]
        public char Separator { get; set; }
        [Parameter(Description = "Колонка в таблице: Дата")]
        public string Col_Date { get; set; }
        [Parameter(Description = "Формат даты")]
        public string FormatDate { get; set; }
        [Parameter(Description = "Колонка в таблице: Время")]
        public string Col_Time { get; set; }
        [Parameter(Description = "Формат времени")]
        public string FormatTime { get; set; }
        [Parameter(Description = "Колонка в таблице: Код бумаги")]
        public string Col_TickerCode { get; set; }
        [Parameter(Description = "Колонка в таблице: Цена")]
        public string Col_Price { get; set; }
        [Parameter(Description = "Колонка в таблице: Кол-во")]
        public string Col_Qty { get; set; }
        [Parameter(Description = "Объем (если не заполнено количество)")]
        public string Col_Volume { get; set; } 
        [Parameter(Description = "Колонка в таблице: Период")]
        public string Col_TradePeriod { get; set; }
        [Parameter(Description = "Значение периода: Открытие")]
        public string Val_PeriodOpening { get; set; } 
        [Parameter(Description = "Значение периода: Нормальный")]
        public string Val_PeriodTrading { get; set; } 
        [Parameter(Description = "Значение периода: Закрытие")]
        public string Val_PeriodClosing { get; set; } 
        [Parameter(Description = "Время начала сессии(если нет колонки период) h:m:s")]
        public TimeSpan Val_SessionStart { get; set; } 
        [Parameter(Description = "Время окнчания сессии(если нет колонки период) h:m:s")]
        public TimeSpan Val_SessionFinish { get; set; }
        [Parameter(Description = "Задержка. Во сколько раз быстрее реального времени. 0 - без задержки")]
        public int Delay { get; set; }

        #endregion

        public event EventHandler<TickEventArgs> TickPoked;

        public CsvFileConnector()
            :this(CurrentRepository.TickerRepository)
        {
        }

        public CsvFileConnector(ITickerRepository tickerRepository)
        {
            this.tickerRepository = tickerRepository;

            FilePath = @"C:\Projects\RMarketMVCgit\RMarketMVC\RMarket.Examples\files\SBER_160601_160601.csv";
            Separator = ';';
            Col_Date = "<DATE>";
            FormatDate = "yyyyMMdd";
            Col_Time = "<TIME>";
            FormatTime = "HHmmss";
            Col_TickerCode = "<TICKER>";
            Col_Price = "<LAST>";
            Col_Qty = "Qty";
            Col_Volume = "<VOL>";
            Col_TradePeriod = "Period";
            Val_PeriodOpening = "Opening";
            Val_PeriodTrading = "Trading";
            Val_PeriodClosing = "Closing";
            Val_SessionStart = new TimeSpan(10, 0, 0);
            Val_SessionFinish = new TimeSpan(19, 0, 0);

            cts = new CancellationTokenSource();
        }

        public void StartServer()
        {
            TaskFactory factory = new TaskFactory(cts.Token);
            factory.StartNew(() =>
            {

                //запустить обход файла
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    Dictionary<string, Ticker> tickersCache = new Dictionary<string, Ticker>();

                    DateTime lastMinute = new DateTime();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (cts.Token.IsCancellationRequested) //выход из потока по кнопке "Стоп"
                            return;

                        string[] cells = line.Split(Separator);

                        bool isRealTime = true;

                        ConnectorHelper helper = new ConnectorHelper();

                        if (headTable == null) //Должна быть строка с заголовками
                        {
                            headTable = helper.FillHeadTable(cells);
                            continue;
                        }

                        //try
                        //{
                            TickEventArgs tick = new TickEventArgs();
                            tick.Date = helper.ParseDate(cells, headTable, Col_Date, Col_Time, FormatDate, FormatTime);
                            tick.TickerCode = helper.ParseTickerCode(cells, headTable, Col_TickerCode);
                            tick.Price = helper.ParsePrice(cells, headTable, Col_Price, CultureInfo.InvariantCulture);
                            tick.Quantity = helper.ParseQuantity(cells, headTable, Col_Qty);
                            if (tick.Quantity == 0)
                            {
                                if(!tickersCache.ContainsKey(tick.TickerCode))
                                    tickersCache[tick.TickerCode] = tickerRepository.Tickers.FirstOrDefault(t => t.Code == tick.TickerCode);

                                Ticker ticker = tickersCache[tick.TickerCode];
                                if (ticker != null && ticker.QtyInLot.HasValue)
                                    tick.Quantity = helper.ParseQuantity(cells, headTable, Col_Volume, ticker.QtyInLot.Value);
                            }
                            tick.IsRealTime = isRealTime;

                            tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, Col_TradePeriod, Val_PeriodOpening, Val_PeriodTrading, Val_PeriodClosing);
                            if (tick.TradePeriod == TradePeriod.Undefended)
                                tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, Col_Time, FormatTime, Val_SessionStart, Val_SessionFinish);

                            tick.Extended = helper.CreateExtended(cells, headTable);

                        //Включаем задержку
                        if(Delay != 0 && lastMinute < tick.Date)
                        {
                            Thread.Sleep(60000 / Delay);
                            lastMinute = tick.Date.AddMinutes(1);
                        }

                            //вызвать событие IConnector
                            var tickPoked = TickPoked;
                            tickPoked?.Invoke(this, tick);

                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}

                    }

                }
            });

        }
        public void StopServer()
        {
            cts.Cancel();
        }


    }
}
