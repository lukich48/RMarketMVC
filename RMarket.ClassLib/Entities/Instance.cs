namespace RMarket.ClassLib.Entities
{
    using Abstract;
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [MetadataType(typeof(Instance_metadata))]
    public class Instance: IEntityData, IEntitySetting
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public int EntityInfoId { get; set; }

        public int TickerId { get; set; }

        public int TimeFrameId { get; set; }

        public decimal Balance { get; set; }

        public decimal Slippage { get; set; }

        public decimal Rent { get; set; }

        public string Description { get; set; }

        public Guid GroupID { get; set; }

        public DateTime CreateDate { get; set; }

        public string StrParams { get; set; }

        public int? SelectionId { get; set; }

        public EntityInfo EntityInfo { get; set; }

        public Ticker Ticker { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public Selection Selection { get; set; }

    }
}
