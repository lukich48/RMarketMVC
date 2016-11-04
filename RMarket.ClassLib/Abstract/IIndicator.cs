using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    public interface IIndicator: IDependency
    {
        /// <summary>
        /// Значения индикатора
        /// </summary>
        Dictionary<string, IndicatorResult> Results { get; set; }

        /// <summary>
        /// Метод обработчик события формирования свечи в соотв. объекте Instrument.
        /// В данном методе рекомендуется заполнять коллекцию Values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddValue(object sender, EventArgs e);
    }
}


