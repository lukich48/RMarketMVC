using System;
using RMarket.ClassLib.Entities;

namespace RMarket.ClassLib.Abstract
{
    public interface IHistoricalProvider: IDependency
    {
        /// <summary>
        ///  Должен заменять в базе данных значения за выбранный диапазон дат
        /// </summary>
        /// <param name="dateFrom">Приводится к началу дня</param>
        /// <param name="dateTo">Приводится к началу дня. Не включая!</param>
        /// <param name="ticker"></param>
        /// <param name="timeFrame"></param>
        /// <returns>Количество записанных записей</returns>
        int DownloadAndSave(DateTime dateFrom, DateTime dateTo, Ticker ticker, TimeFrame timeFrame);
    }
}
