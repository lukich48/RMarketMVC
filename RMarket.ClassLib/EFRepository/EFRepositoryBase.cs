using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.EFRepository
{
    public class EFRepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity:class, IEntityData
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

        public virtual IEnumerable<TEntity> Get1()
        {
            return Context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get1(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression)
        {            
            IQueryable<TEntity> dataCollection = expression.Compile()(Context.Set<TEntity>());
            return dataCollection;
        }

        public virtual IEnumerable<TResult> Get1<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression)
        {
            IEnumerable<TResult> dataCollection = expression.Compile()(Context.Set<TEntity>()).ToList();

            return dataCollection;
        }

        public virtual TEntity GetById1(int id, bool includeAll)
        {
            TEntity data = null;

            if (includeAll)
                data = Context.Set<TEntity>().IncludeAll().SingleOrDefault(i => i.Id == id);
            else
                data = Context.Set<TEntity>().Find(id);

            return data;
        }

        public virtual TEntity GetById1(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            TEntity data = null;

            if (includeProperties.Length > 0)
                data = Context.Set<TEntity>().IncludeProperties(includeProperties).SingleOrDefault(i => i.Id == id);
            else
                data = Context.Set<TEntity>().Find(id);

            return data;
        }

        public virtual void Save (TEntity data)
        {
            if (data.Id == 0)
            {
                context.Set<TEntity>().Add(data);
            }
            else
            {
                context.Entry(data).State = EntityState.Modified;
            }

            context.SaveChanges();
        }



    }
}
