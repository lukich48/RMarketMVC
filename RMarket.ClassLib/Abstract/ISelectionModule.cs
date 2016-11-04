using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    //todo: не используется
    public interface ISelectionModule
    {
        IEnumerable<Instance> SelectInstance(Selection selection, IEnumerable<ParamSelection> selectionParams);
    }
}
