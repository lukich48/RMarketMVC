using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMarket.WebUI.Models
{

    [MetadataType(typeof(DataProviderSetting_metadata))]
    public class OptimizationSettingModelUI
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EntityInfoId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ParamEntityUI> EntityParams { get; set; }

        public IEntityInfo EntityInfo { get; set; }

        public OptimizationSettingModelUI()
        {
            EntityParams = new List<ParamEntityUI>();
        }

    }
}
