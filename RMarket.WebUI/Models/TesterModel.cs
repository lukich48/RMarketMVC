using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Infrastructure.HighChart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace RMarket.WebUI.Models
{
    public class TesterModel
    {
        [Display(Name = "Вариант")]
        public int InstanceId { get; set; }
        [Display(Name = "Коннектор")]
        public int SettingId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

    }

}