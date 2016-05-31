using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    /// <summary>
    /// интерфейс позволяет создавать из объекта описания объект указанного типа
    /// </summary>
    public interface IEntityInfo
    {
        string Name { get; set; }
        string TypeName { get; set; }
    }
}
