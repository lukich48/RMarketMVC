using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFSelectionRepository: EFRepositoryBase<Selection>, ISelectionRepository
    {
        public EFSelectionRepository(RMarketContext context)
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
