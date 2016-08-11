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
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.DataAccess.Repositories
{
    public class EFSelectionRepository: EFRepositoryBase<Selection>, ISelectionRepository
    {
        public EFSelectionRepository(Context.RMarketContext context)
            :base(context)
        { }

       
        public override void Save(Selection selection)
        {
            selection.CreateDate = DateTime.Now;

            if (selection.Id == 0) //Insert
            {
                selection.GroupID = Guid.NewGuid();
            }
            else //Update
            { }

            context.Selections.Add(selection);
            context.SaveChanges();
        }

    }
}
