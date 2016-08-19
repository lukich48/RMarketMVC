using System;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;

namespace RMarket.DataAccess.Repositories
{
    public class EFInstanceRepository : EFRepositoryBase<Instance>, IInstanceRepository
    {
        public EFInstanceRepository(RMarketContext context)
            :base(context)
        { }

        public override void Save(Instance instance)
        {
            instance.CreateDate = DateTime.Now;

            if (instance.Id == 0) //Insert
            {
                instance.GroupID = Guid.NewGuid();
            }
            else //Update
            { }

            context.Instances.Add(instance);
            context.SaveChanges();
        }

    }
}
