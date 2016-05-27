using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMarket.ClassLib.Abstract
{
    public interface ISelectionRepository: IRepositoryBase<Selection,SelectionModel>, IDisposable
    {
    }
}
