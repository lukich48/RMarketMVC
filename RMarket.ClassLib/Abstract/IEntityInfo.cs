using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    public interface IEntityInfo
    {
        string Name { get; set; }
        string TypeName { get; set; }
    }
}
