using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;

namespace RMarket.ClassLib.Models
{
    public abstract class HistoricalProviderBase:IHistoricalProvider
    {
        public ICandleRepository candleRepository = Current.CandleRepository;

        public virtual int DownloadAndSave(DateTime dateFrom, DateTime dateTo, Ticker ticker, TimeFrame timeFrame)
        {
            int res = 0;

            IEnumerable<Candle> newCandles = DownloadCandles(dateFrom, dateTo, ticker, timeFrame);

            if (newCandles != null && newCandles.Count()>0)
            {
                //Удалить из базы данные
                IEnumerable<Candle> oldCandles = from c in candleRepository.Candles
                                          where c.TickerId == ticker.Id && c.TimeFrameId == timeFrame.Id && c.DateOpen >= dateFrom.Date && c.DateOpen < dateTo
                                          select c;
                candleRepository.RemoveRange(oldCandles);
                //

                candleRepository.AddRange(newCandles);

                res = newCandles.Count();
            }

            return res;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateFrom">Учитывается дата</param>
        /// <param name="dateTo">Не включая!</param>
        /// <param name="ticker"></param>
        /// <param name="timeFrame"></param>
        /// <returns></returns>
        public abstract IEnumerable<Candle> DownloadCandles(DateTime dateFrom, DateTime dateTo, Ticker ticker, TimeFrame timeFrame);

    }
}
