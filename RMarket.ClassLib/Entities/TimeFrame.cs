namespace RMarket.ClassLib.Entities
{
    using Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TimeFrame : IEntityData
    {
 
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public int ToMinute { get; set; }

        [StringLength(20)]
        public string CodeFinam { get; set; }

    }
}
