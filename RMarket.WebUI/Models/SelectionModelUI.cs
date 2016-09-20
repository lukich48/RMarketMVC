using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
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
    [MetadataType(typeof(Selection_metadata))]
    public class SelectionModelUI
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

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int AmountResults { get; set; }

        public List<ParamSelectionUI> SelectionParams { get; set; }

        public EntityInfo EntityInfo { get; set; }

        public Ticker Ticker { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public SelectionModelUI()
        {
            SelectionParams = new List<ParamSelectionUI>();
        }

    }
}
