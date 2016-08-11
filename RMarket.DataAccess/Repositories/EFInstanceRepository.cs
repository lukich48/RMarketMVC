using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Helpers.Extentions;
using System.Linq.Expressions;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.EntityModels;
using RMarket.DataAccess.Context;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.DataAccess.Repositories
{
    public class EFInstanceRepository : EFRepositoryBase<Instance>, IInstanceRepository
    {
        public EFInstanceRepository(Context.RMarketContext context)
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
