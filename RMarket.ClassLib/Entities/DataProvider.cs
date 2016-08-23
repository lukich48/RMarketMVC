using RMarket.ClassLib.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Entities
{

    [MetadataType(typeof(DataProvider_metadata))]
    public class DataProvider : IEntityData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ConnectorInfoId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public string StrParams { get; set; }

        public ConnectorInfo ConnectorInfo { get; set; }
    }
}
