using RMarket.ClassLib.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    public interface IOptimization
    {
        /// <summary>
        /// трартует оптимизацию параметров
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>количество лучших результатов определяется в объекте selection.AmountResults</returns>
        List<InstanceModel> Start(SelectionModel selection, DateTime dateFrom, DateTime dateTo);

    }
}
