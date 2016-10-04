using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMarket.ClassLib.EntityModels
{

    [MetadataType(typeof(OptimizationSetting_metadata))]
    public class OptimizationSettingModel : ISettingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EntityInfoId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ParamEntity> EntityParams { get; set; }

        public IEntityInfo EntityInfo { get; set; }

        public OptimizationSettingModel()
        {
            EntityParams = new List<ParamEntity>();
        }

    }
}
