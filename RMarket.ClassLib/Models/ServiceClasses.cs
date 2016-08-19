using System;
using System.Collections.Generic;

namespace RMarket.ClassLib.Models
{
    public enum TradePeriod : int
    {
        Undefended = 0,
        Opening = 1,
        Trading = 2,
        Closing = 3
    }

    public enum AliveType
    {
        Real = 1,
        Emul = 2,
        Test = 3
    }

    #region Классы для индикаторов

    public class IndicatorResult
    {
        /// <summary>
        /// Период линии индикатора
        /// </summary>
        public int Period { get; set; }
        public List<IndicatorValue> Values { get; set; }

        public IndicatorResult(int period)
        {
            this.Period = period;
            Values = new List<IndicatorValue>();
        }
    }

    public class IndicatorValue
    {
        /// <summary>
        /// Дата открытия свечи
        /// </summary>
        public DateTime DateOpen { get; set; }
        public decimal Value { get; set; }


    }

    #endregion

    #region Классы для Терминалов
    public class TickEventArgs : EventArgs
    {
        public DateTime Date { get; set; }
        /// <summary>
        /// код бумаги
        /// </summary>
        public string TickerCode { get; set; }
        /// <summary>
        /// цена
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// количество лотов
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// указывает на то, что данные актуальны на данный момент
        /// </summary>
        public bool IsRealTime { get; set; }
        /// <summary>
        /// Работаем только в торговую сессию
        /// </summary>
        public TradePeriod TradePeriod { get; set; }
        /// <summary>
        /// Записывается вся информация, которая может понадобиться
        /// </summary>
        public Dictionary<string, string> Extended { get; set; }

        //public TickEventArgs()
        //{
        //    Extended = new Dictionary<string, string>();
        //}

    }

    #endregion

    #region Атрибуты

    /// <summary>
    /// Атрибут параметров стратегии. Поля помеченные этим атрибутам можно изменить из интерфейса до старта стратегии
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ParameterAttribute : System.Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Аттрибут навешивается на поля стратегии с типом IIndicator. Для отображения индикатора на графике 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DisplayChartAttribute : System.Attribute
    {
        public string Name { get; set; }
    }

    #endregion



}