using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Entities
{
    [System.Web.Mvc.Bind(Exclude = "StrategyInfo")]
    public class Setting_metadata
    {
        [Display(Name = "Имя настройки")]
        [StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Стратегия")]
        public int? StrategyInfoId { get; set; }

        [Display(Name = "Тип настройки")]
        public SettingType SettingType { get; set; }

        /// <summary>
        /// ид сущности, к которой относятся настройки
        /// </summary>
        [Display(Name = "Объект настроек")]
        [Required(ErrorMessage = "Укажите объект для настроек")]
        public int EntityInfoId { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [StringLength(2000)]
        public string Description { get; set; }

    }
}
