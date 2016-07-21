using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Models
{
    public class AliveResult
    {
        public int AliveId { get; set; }
        public DateTime StartDate { get; set; }
        public InstanceModel Instance { get; set; }
        public IStrategy Strategy { get; set; }
        public IManager Manager { get; set; }
        //Ключ - имя индикатора
        public Dictionary<string, IIndicator> IndicatorsDict { get; set; }

    }
}
