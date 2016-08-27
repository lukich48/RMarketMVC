using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Entities
{
    [System.Web.Mvc.Bind(Exclude = "EntityInfo,Ticker,TimeFrame")]
    public class Selection_metadata
    {
        [Display(Name = "Имя варианта")]
        [Required(ErrorMessage = "Введите имя экземпляра")]
        [StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Стратегия")]
        [Required(ErrorMessage = "Поле стратегия не может бть пустым")]
        public string EntityInfoId { get; set; }

        [Display(Name = "Инструмент")]
        [Required(ErrorMessage = "Инструмент не может быть пустым")]
        public string TickerId { get; set; }

        [Display(Name = "Тайм-фрейм")]
        [Required(ErrorMessage = "Тайм-фрейм не может быть пустым")]
        public int TimeFrameId { get; set; }

        [Display(Name = "Размер портфеля")]
        [Range(typeof(Decimal), "1", "100000000000", ErrorMessage = "Значение должно быть больше 0")]
        public decimal Balance { get; set; }

        [Display(Name = "Проскальзывание (Для эмуляции)")]
        [Range(typeof(Decimal), "0", "1", ErrorMessage = "Значение от 0 до 1")]
        public decimal Slippage { get; set; }

        [Display(Name = "Комиссия, % (Для эмуляции)")]
        [Range(typeof(Decimal), "0", "100", ErrorMessage = "Значение от 0 до 100")]
        public decimal Rent { get; set; }

        [Display(Name="Дата с")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Дата по")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Количество результатов")]
        [Range(typeof(int), "1", "10", ErrorMessage = "Значение от 1 до 10")]
        public int AmountResults { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [StringLength(2000)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
