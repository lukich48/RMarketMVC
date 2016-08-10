namespace RMarket.ClassLib.Entities
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StrategyInfo : IEntityData, IEntityInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string TypeName { get; set; }


        //public virtual ICollection<Instance> Instances { get; set; }
    }
}
