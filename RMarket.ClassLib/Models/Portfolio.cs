using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.ClassLib.Models
{
    /// <summary>
    /// Класс для работы с торговым счетом
    /// </summary>
    public class Portfolio
    {
        public decimal Balance { get; set; }

        /// <summary>
        /// Комиссия в %
        /// </summary>
        public decimal Rent { get; set; }

        /// <summary>
        /// Проскальзывание в пунктах. Для тестового режима
        /// </summary>
        public decimal Slippage { get; set; }

    }
}