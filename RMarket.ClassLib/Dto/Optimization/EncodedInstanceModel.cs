using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Models;
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
        public IList<EncodedEntityParam> EncodedEntityParams { get; set; }

    }
}
