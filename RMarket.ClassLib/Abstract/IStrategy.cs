using System;
using System.Collections.Generic;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Entities;

namespace RMarket.ClassLib.Abstract
{
    /// <summary>
    /// Интерфейс реализуют пользовательские стратегии
    /// </summary>
    public interface IStrategy
    {
        Instrument Instr { get; set; }
        IManager Manager { get; set; }
        /// <summary>
        /// Выставленные в текущей стратегии ордера. После рестарта подгружаются только открытые.  
        /// </summary>
        List<Order> Orders { get; set; }


        /// <summary>
        /// Метод выполняется единожды после создания объекта
        /// В нем возможно инициировать индикаторы
        /// </summary>
        void Initialize();

        /// <summary>
        /// Происходит при формировании свечи
        /// </summary>
        void Begin();

        /// <summary>
        /// В метод попадают все данные из таблицы всех сделок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTickPoked(object sender, TickEventArgs e);
    }
}
