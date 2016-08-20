using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMarket.ClassLib.EntityModels
{

    [MetadataType(typeof(Setting_metadata))]
    public class SettingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? StrategyInfoId { get; set; }

        public SettingType SettingType { get; set; }

        public int EntityInfoId { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ParamEntity> EntityParams { get; set; }

        public StrategyInfo StrategyInfo { get; set; }

        public IEntityInfo EntityInfo { get; set; }

        public SettingModel()
        {
            EntityParams = new List<ParamEntity>();
        }

    }
}
