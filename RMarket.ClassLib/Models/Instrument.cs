using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using RMarket.ClassLib.Entities;
using System.Globalization;
using RMarket.ClassLib.Abstract;
using System.Collections;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Infrastructure;

namespace RMarket.ClassLib.Models
{
    /// <summary>
    /// В экземпляре данного класса содержится информация об инструменте.
    /// </summary>
    public class Instrument
    {
        //начало дня
        private TimeSpan startTime = new TimeSpan(10, 0, 0);

        public ITickerRepository tickerRepository = Current.TickerRepository;

        public Ticker Ticker { get; set; }
        public TimeFrame TimeFrame { get; set; }

        /// <summary>
        /// Хранятся свечи
        /// </summary>
        public List<Candle> Candles { get; internal set; }
        /// <summary>
        /// Нулевой бар. Текущая свеча, параметры которой постоянно обновляются
        /// </summary>
        public Candle ZeroBar { get; internal set; }

        /// <summary>
        /// Пришел тик
        /// </summary>
        public event EventHandler<TickEventArgs> TickPoked;
        /// <summary>
        /// свеча закрылась. На событие подписываются индикаторы
        /// </summary>
        public event EventHandler CreatedCandle;

        private List<Action<object>> evntAsyncList = new List<Action<object>>();
        public event Action<object> CreatedCandleAsync
        {
            add { evntAsyncList.Add(value); }
            remove { evntAsyncList.Remove(value); }
        }

        /// <summary>
        /// На это событие подписывается стратегия
        /// </summary>
        public event EventHandler CreatedCandleReal;

        private Instrument() { }
        public Instrument (Ticker ticker, TimeFrame timeFrame)
        {
            this.Ticker = ticker;
            this.TimeFrame = timeFrame;

            Candles = new List<Candle>();
        }

        /// <summary>
        /// Свеча сформировалась
        /// </summary>
        public void OnCreatedCandle()
        {
            if (CreatedCandle!=null)
                CreatedCandle(this, null);
        }

        /// <summary>
        /// Свеча сформировалась. Ассинхронный обработчик
        /// </summary>
        public void OnCreatedCandleAsync()
        {
            if (evntAsyncList.Count>0)
            {
                Task[] tasks= new Task[evntAsyncList.Count];
                for (int i=0; i<evntAsyncList.Count; i++)
                {
                    Task task = Task.Factory.StartNew(evntAsyncList[i], this);
                    tasks[i]=task;
                }
                Task.WaitAll(tasks);
            }
                
        }

        /// <summary>
        /// все индикаторы сформированы. Здесь подписывается стратегия
        /// </summary>
        public void OnCreatedCandleComplete()
        {
            if (CreatedCandleReal != null)
                CreatedCandleReal(this, null);
        }

        public void OnTickPoked(object sender, TickEventArgs tick)
        {

            if (TickPoked != null) //todo: Сделать ассинхронно
                TickPoked(this, tick);

            if (tick.TradePeriod != TradePeriodEnum.Undefended && tick.TradePeriod != TradePeriodEnum.Trading)
                return; //торгуем только в сессию!

            //Заполнить "нулевой бар"
            if (Ticker.Code == tick.TickerCode)
            {
                DateTime nextTime;
                if (ZeroBar == null)
                    nextTime = tick.Date.Date + startTime;
                else
                    nextTime = ZeroBar.DateOpen.AddMinutes(TimeFrame.ToMinute);

                if (tick.Date >= nextTime)
                {
                    //Добавляем свечу в коллекцию
                    if (ZeroBar != null)
                    {
                        Candles.Insert(0, ZeroBar);
                        //вызвать событие формирования свечи
                        OnCreatedCandle();
                        OnCreatedCandleAsync();

                        if (tick.IsRealTime) //Стратегию вызываем только в реалтайме!
                        {
                            if (CreatedCandleReal != null)
                                CreatedCandleReal(this, null);
                        }
                            //Strategy.Begin();
                    }

                    ZeroBar = new Candle();
                    ZeroBar.DateOpen = nextTime;
                    ZeroBar.Ticker = Ticker;
                    ZeroBar.TimeFrame = TimeFrame;

                    ZeroBar.OpenPrice =
                    ZeroBar.HighPrice =
                    ZeroBar.LowPrice =
                    ZeroBar.ClosePrice = tick.Price;

                }
                else if (ZeroBar != null)
                {
                    ZeroBar.HighPrice = Math.Max(ZeroBar.HighPrice, tick.Price);
                    ZeroBar.LowPrice = Math.Min(ZeroBar.LowPrice, tick.Price);
                    ZeroBar.ClosePrice = tick.Price;
                }

            }
            
        }

    }
}