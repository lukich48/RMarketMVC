using RMarket.ClassLib.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace RMarket.ClassLib.Entities
{

    [MetadataType(typeof(HistoricalProviderSetting_metadata))]
    public class HistoricalProviderSetting : IEntityData, IEntitySetting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EntityInfoId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public string StrParams { get; set; }

        public EntityInfo EntityInfo { get; set; }
    }
}
