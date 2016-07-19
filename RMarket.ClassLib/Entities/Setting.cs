using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Entities
{
    public enum SettingType
    {
        None = 0,
        ConnectorInfo = 1
    }

    [MetadataType(typeof(Setting_metadata))]
    public class Setting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? StrategyInfoId { get; set; }

        public SettingType SettingType { get; set; }

        public int EntityInfoId { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime CreateDate { get; set; }

        public string StrParams { get; set; }

        public virtual StrategyInfo StrategyInfo { get; set; }
    }
}
