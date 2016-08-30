using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Entities
{
    [System.Web.Mvc.Bind(Exclude = "EntityInfo")]
    public class HistoricalProviderSetting_metadata
    {
        [Display(Name = "Имя настройки")]
        [StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Объект настроек")]
        [Required(ErrorMessage = "Укажите объект для настроек")]
        public int EntityInfoId { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [StringLength(2000)]
        public string Description { get; set; }

    }
}
