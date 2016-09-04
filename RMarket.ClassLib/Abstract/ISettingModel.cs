using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    public interface ISettingModel
    {
        int Id { get; set; }
        int EntityInfoId { get; set; }
        IEntityInfo EntityInfo { get; set; }
        List<ParamEntity> EntityParams { get; set; }
    }
}
