using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Models
{
    /// <summary>
    /// Рекомендуется от данного класса наследовать все индикаторы и реализовать в них метод Begin()
    /// </summary>
    public abstract class IndicatorBase : IIndicator
    {
        public Instrument Instr { get; private set; }

        /// <summary>
        /// Период индикатора. Значение по умолчанию 0
        /// Если у индикатора несколько периодов, это - максимальный
        /// </summary>
        //public int Period { get; private set; }

        /// <summary>
        /// Значения индикатора
        /// </summary>
        public Dictionary<string, IndicatorResult> Results { get; set; }

        /// <summary>
        /// Вызывайте данный конструктор при инициализации собственного индикатора
        /// </summary>
        /// <param name="instr"></param>
        protected IndicatorBase(Instrument instr)
        {
            this.Results = new Dictionary<string, IndicatorResult>();
            this.Instr = instr;
            Instr.CreatedCandle += AddValue;
        }

        public virtual void AddValue(object sender, EventArgs e)
        {
            Candle curCandle = (sender as Instrument).Candles[0];

            Dictionary<string, decimal> dictResult = Begin();
            foreach(KeyValuePair<string, decimal> pair in dictResult)
            {
                IndicatorValue res = new IndicatorValue();
                res.DateOpen = curCandle.DateOpen;
                res.Value = pair.Value;

                Results[pair.Key].Values.Insert(0, res);
            }

        }

        /// <summary>
        /// Метод который происхоит при формировании свечи
        /// </summary>
        /// <returns> Key - название линии, Value - значение точки </returns>
        protected abstract Dictionary<string, decimal> Begin();

    }
}
