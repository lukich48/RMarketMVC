using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Models
{
    /// <summary>
    /// Класс содержит результаты прогона стратегии на истории
    /// </summary>
    public class StrategyResult
    {
        InstanceModel Instance { get; set; }
        decimal Profit { get; set; }
        IEnumerable<ResultProfit> ProfitList { get; set; }

        class ResultProfit
        {
            DateTime DateOpen { get; set; }
            DateTime DateClose { get; set; }
            decimal Profit { get; set; }
        }
    }
}
