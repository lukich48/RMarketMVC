using RMarket.ClassLib.EntityModels;
using System.Collections.Generic;

namespace RMarket.Concrete.Optimization.Dto
{
    public class EncodedInstanceModel
    {
        public InstanceModel Instance { get; set; }
        public IList<EncodedEntityParam> EncodedEntityParams { get; set; }

    }
}
