using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public class ConnectorHelper
    {

        /// <summary>
        /// заполнить заголовки таблицы
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public Dictionary<string, int> FillHeadTable(object[] cells)
        {
            Dictionary<string, int> headTable = new Dictionary<string, int>();

            for (int i = 0; i < cells.Length; i++)
            {
                string cell = cells[i] as string;
                headTable.Add(cell, i);
            }

            //!!!проверка на наличие всех обязательных колонок

            return headTable;
        }

        public DateTime ParseDate(object[] cells, Dictionary<string, int> headTable, string col_Date, string col_Time, string formatDate, string formatTime)
        {
            return headTable.ContainsKey(col_Date) ? DateTime.ParseExact(cells[headTable[col_Date]].ToString() + cells[headTable[col_Time]].ToString(), formatDate + formatTime, null) :
                                                                DateTime.ParseExact(cells[headTable[col_Time]].ToString(), formatTime, null);
        }

        public string ParseTickerCode(object[] cells, Dictionary<string, int> headTable, string col_TickerCode)
        {
            return cells[headTable[col_TickerCode]].ToString();
        }

        public decimal ParsePrice(object[] cells, Dictionary<string, int> headTable, string col_Price, IFormatProvider cultureInfo)
        {
            return Convert.ToDecimal(cells[headTable[col_Price]], cultureInfo);
        }

        public int ParseQuantity(object[] cells, Dictionary<string, int> headTable, string col_Qty)
        {
            int qty;

            int.TryParse(cells[headTable[col_Qty]].ToString(), out qty);
            return qty;
        }

        public int ParseQuantity(object[] cells, Dictionary<string, int> headTable, string col_Volume, int qtyInLot)
        {
            int val;

            int.TryParse(cells[headTable[col_Volume]].ToString(), out val);

            return val / qtyInLot;
        }

        public TradePeriodEnum ParseTradePeriod(object[] cells, Dictionary<string, int> headTable, string col_TradePeriod, string val_PeriodOpening, string val_PeriodTrading, string val_PeriodClosing)
        {
            TradePeriodEnum tradePeriod = TradePeriodEnum.Undefended;
            if (headTable.ContainsKey(col_TradePeriod))
            {
                string curPeriod = cells[headTable[col_TradePeriod]].ToString();

                if (string.Equals(curPeriod, val_PeriodOpening, StringComparison.OrdinalIgnoreCase))
                    tradePeriod = TradePeriodEnum.Opening;
                else if (string.Equals(curPeriod, val_PeriodTrading, StringComparison.OrdinalIgnoreCase))
                    tradePeriod = TradePeriodEnum.Trading;
                else if (string.Equals(curPeriod, val_PeriodClosing, StringComparison.OrdinalIgnoreCase))
                    tradePeriod = TradePeriodEnum.Closing;
            }

            return tradePeriod;
        }

        public TradePeriodEnum ParseTradePeriod(object[] cells, Dictionary<string, int> headTable, string col_Time, string formatTime, TimeSpan val_SessionStart, TimeSpan val_SessionFinish)
        {
            TradePeriodEnum tradePeriod = TradePeriodEnum.Trading;

            TimeSpan curTime = TimeSpan.ParseExact(cells[headTable[col_Time]].ToString(), formatTime, null);
            if (curTime < val_SessionStart)
                tradePeriod = TradePeriodEnum.Opening;
            else if (curTime > val_SessionFinish)
                tradePeriod = TradePeriodEnum.Closing;

            return tradePeriod;
        }

        public Dictionary<string, string> CreateExtended (object[] cells, Dictionary<string, int> headTable)
        {
            Dictionary<string, string> extended = new Dictionary<string, string>();

            foreach (var row in headTable)
            {
                extended.Add(row.Key, cells[row.Value].ToString());
            }

            return extended;
        }
    }
}
