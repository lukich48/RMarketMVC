using RMarket.ClassLib.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Dto.Optimization
{
    public class EncodedInstanceModel
    {
        public InstanceModel Instance { get; set; }
        public ICollection<EncodedEntityParam> EncodedEntityParams { get; set; }

        public EncodedInstanceModel()
        {
            EncodedEntityParams = new List<EncodedEntityParam>();
        }
    }
}
