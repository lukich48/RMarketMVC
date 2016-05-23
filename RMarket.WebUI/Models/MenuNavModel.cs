using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Models
{
    public class MenuNavModel
    {
        public StrategyInfo StrategyInfo { get; set; }
        /// <summary>
        /// количество экземпляров
        /// </summary>
        public int Count { get; set; }
    }
}