using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Infrastructure;
using System.Linq.Expressions;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.ClassLib.Services
{
    public class SelectionService: EntityServiceBase<Selection,SelectionModel>, ISelectionService
    {
        public SelectionService(ISelectionRepository selectionRepository)
            :base(selectionRepository)
        { }
    }
}
