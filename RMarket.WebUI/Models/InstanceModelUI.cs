using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.WebUI.Models
{
    [MetadataType(typeof(Instance_metadata))]
    public class InstanceModelUI
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

        public int? SelectionId { get; set; }

        public List<ParamEntityUI> EntityParams { get; set; }

        public IEntityInfo EntityInfo { get; set; }

        public Ticker Ticker { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public SelectionModel Selection { get; set; }

        public InstanceModelUI()
        {
            EntityParams = new List<ParamEntityUI>();
        }

    }
}
