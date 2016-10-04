namespace RMarket.ClassLib.Entities
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public enum EntityType
    {
        StrategyInfo = 1,
        DataProviderInfo = 2,
        HistoricalProviderInfo = 3,
        OptimizationInfo = 4,
    }

    public partial class EntityInfo : IEntityData, IEntityInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string TypeName { get; set; }

        public EntityType EntityType { get; set; }
    }

}
