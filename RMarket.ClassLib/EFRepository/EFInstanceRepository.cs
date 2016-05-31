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
    public class EFInstanceRepository : IInstanceRepository
    {
        private RMarketContext context = CurrentRepository.Context;

        public RMarketContext Context
        {
            get
            {
                return context;
            }

            set
            {
                context = value;
            }
        }

        public IEnumerable<InstanceModel> Get()
        {
            return Current.Mapper.Map<IQueryable<Instance>, IEnumerable<InstanceModel>>(Context.Instances);
        }

        public IEnumerable<InstanceModel> Get(Expression<Func<IQueryable<Instance>,IQueryable<Instance>>> expression)
        {
            IQueryable<Instance> dataCollection = expression.Compile()(Context.Instances);

            return Current.Mapper.Map<IQueryable<Instance>, IEnumerable<InstanceModel>>(dataCollection);
        }

        public IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<Instance>, IQueryable<TResult>>> expression)
        {
            IEnumerable<TResult> dataCollection = expression.Compile()(Context.Instances).ToList();

            return dataCollection;
        }

        public InstanceModel GetById(int id, bool includeAll)
        {
            Instance data = null;

            if(includeAll)
                data = Context.Instances.IncludeAll().SingleOrDefault(i => i.Id == id);
            else
                data = Context.Instances.Find(id);
            
            InstanceModel instance = Current.Mapper.Map<Instance, InstanceModel>(data);
    
            return instance;
        }

        public InstanceModel GetById(int id, params Expression<Func<Instance, object>>[] includeProperties)
        {
            Instance data = null;

            if (includeProperties.Length > 0)
                data = Context.Instances.IncludeProperties(includeProperties).SingleOrDefault(i => i.Id == id);
            else
                data = Context.Instances.Find(id);

            InstanceModel instance = Current.Mapper.Map<Instance, InstanceModel>(data);

            return instance;
        }

        public int Save(InstanceModel instance)
        {
            int res = 0;

            instance.CreateDate = DateTime.Now;

            if (instance.Id == 0) //Insert
            {
                instance.GroupID = Guid.NewGuid();
                res = 1;
            }
            else //Update
            {
                res = 2;
            }

            Instance dto = Current.Mapper.Map<Instance>(instance);

            Context.Instances.Add(dto);
            Context.SaveChanges();

            return res;
        }

        #region IDisposable
        public void Dispose()
        {
            Context.Dispose();
        }
        #endregion
    }
}
