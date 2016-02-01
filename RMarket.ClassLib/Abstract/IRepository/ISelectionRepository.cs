using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMarket.ClassLib.Abstract
{
    public interface ISelectionRepository:IDisposable
    {
        IQueryable<Selection> Selections { get; }
        Selection Find(int id);
        SelectionModel FindModel(int id);
        int Save(Selection selection, IEnumerable<ParamSelection> selectionParams);
        int Save(SelectionModel selection);
    }
}
