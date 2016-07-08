namespace RMarket.ClassLib.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ticker
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Code { get; set; }

        [StringLength(20)]
        public string CodeFinam { get; set; }

        [Display(Name = "Количество в 1 лоте")]
        public int? QtyInLot { get; set; } 

    }
}
