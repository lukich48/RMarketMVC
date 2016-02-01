using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;

namespace RMarket.WebUI.Models
{
    public class DownloadHistoryModel
    {

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Ticker Ticker { get; set; }
        public TimeFrame TimeFrame { get; set; }
        
        public string ProviderName { get; set; }

        public IHistoricalProvider Provider { get; set; }

        public DownloadHistoryModel()
        {

        }

        public int DownloadAndSave()
        {
            int res = 0;

            res = Provider.DownloadAndSave(DateFrom, DateTo, Ticker, TimeFrame);

            return res;
        }
    }
}