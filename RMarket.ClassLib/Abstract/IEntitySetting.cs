using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    public interface IEntitySetting:IEntityData
    {
        int EntityInfoId { get; set; }
        EntityInfo EntityInfo { get; set; }
        string StrParams { get; set; }
    }
}
