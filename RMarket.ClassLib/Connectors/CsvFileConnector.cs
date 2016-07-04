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

namespace RMarket.ClassLib.Connectors
{
    public class CsvFileConnector : IDataProvider
    {
        private ITickerRepository tickerRepository;

        private Dictionary<string, int> headTable = new Dictionary<string, int>();
        private CancellationTokenSource cts;

        #region PARAMS

        [Parameter(Description = "Путь к файлу")]
        string FilePath { get; set; }
        [Parameter(Description = "Разделитель")]
        char Separator { get; set; }
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
        [Parameter(Description = "Объем (если не заполнено количество)")]
        string Col_Volume { get; set; } 
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
            Col_Date = "Date";
            FormatDate = "yyyyMMdd";
            Col_Time = "Time";
            FormatTime = "HHmmss";
            Col_TickerCode = "TICKER";
            Col_Price = "LAST";
            Col_Qty = "Qty";
            Col_Volume = "VOL";
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

                        try
                        {
                            TickEventArgs tick = new TickEventArgs();
                            tick.Date = helper.ParseDate(cells, headTable, Col_Date, Col_Time, FormatDate, FormatTime);
                            tick.TickerCode = helper.ParseTickerCode(cells, headTable, Col_TickerCode);
                            tick.Price = helper.ParsePrice(cells, headTable, Col_Price, CultureInfo.InvariantCulture);
                            tick.Quantity = helper.ParseQuantity(cells, headTable, Col_Qty);
                            if (tick.Quantity == 0)
                            {
                                Ticker ticker = tickerRepository.Tickers.FirstOrDefault(t => t.Code == tick.TickerCode); //!!!проверить кеширование
                                if (ticker != null && ticker.QtyInLot.HasValue)
                                    tick.Quantity = helper.ParseQuantity(cells, headTable, Col_Volume, ticker.QtyInLot.Value);
                            }
                            tick.IsRealTime = isRealTime;

                            tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, Col_TradePeriod, Val_PeriodOpening, Val_PeriodTrading, Val_PeriodClosing);
                            if (tick.TradePeriod == TradePeriodEnum.Undefended)
                                tick.TradePeriod = helper.ParseTradePeriod(cells, headTable, Col_TradePeriod, FormatTime, Val_SessionStart, Val_SessionFinish);

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
            });

        }
        public void StopServer()
        {
            cts.Cancel();
        }


    }
}
