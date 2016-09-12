using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMarket.WebUI.Models
{

    [MetadataType(typeof(HistoricalProviderSetting_metadata))]
    public class HistoricalProviderSettingModelUI
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EntityInfoId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ParamEntityUI> EntityParams { get; set; }

        public IEntityInfo EntityInfo { get; set; }

        public HistoricalProviderSettingModelUI()
        {
            EntityParams = new List<ParamEntityUI>();
        }

    }
}
