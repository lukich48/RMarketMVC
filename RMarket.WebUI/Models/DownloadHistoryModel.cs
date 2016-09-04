using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.EntityModels;
using System.ComponentModel.DataAnnotations;

namespace RMarket.WebUI.Models
{
    public class DownloadHistoryModel
    {
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required]
        public int TickerId { get; set; }
        [Required]
        public int TimeFrameId { get; set; }
        [Required]
        public int SettingId { get; set; }

        public Ticker Ticker { get; set; }
        public TimeFrame TimeFrame { get; set; }
        public HistoricalProviderSettingModel Setting { get; set; }

    }
}