using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RMarket.ClassLib.EntityModels
{

    [MetadataType(typeof(DataProvider_metadata))]
    public class DataProviderModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DataProviderInfoId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ParamEntity> EntityParams { get; set; }

        public IEntityInfo DataProviderInfo { get; set; }

        public DataProviderModel()
        {
            EntityParams = new List<ParamEntity>();
        }

    }
}
