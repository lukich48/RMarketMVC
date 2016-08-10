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

namespace RMarket.ClassLib.EFRepository
{
    public class EFInstanceRepository : EFRepositoryBase<Instance>
    {
        public override void Save(Instance instance)
        {
            instance.CreateDate = DateTime.Now;

            if (instance.Id == 0) //Insert
            {
                instance.GroupID = Guid.NewGuid();
            }
            else //Update
            { }

            Context.Instances.Add(instance);
            Context.SaveChanges();
        }

    }
}
