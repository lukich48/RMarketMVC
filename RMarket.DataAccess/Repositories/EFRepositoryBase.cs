using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Infrastructure;
using RMarket.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.DataAccess.Repositories
{
    public class EFRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity:class, IEntityData
    {
        protected readonly RMarketContext context;

        public EFRepositoryBase(RMarketContext context)
        {
            this.context = context;
        }

        public virtual IEnumerable<TEntity> Get()
        {
            return context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression)
        {            
            IQueryable<TEntity> dataCollection = expression.Compile()(context.Set<TEntity>());
            return dataCollection;
        }

        public virtual IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression)
        {
            IEnumerable<TResult> dataCollection = expression.Compile()(context.Set<TEntity>()).ToList();

            return dataCollection;
        }

        public virtual TEntity GetById(int id, bool includeAll)
        {
            TEntity data = null;

            if (includeAll)
                data = context.Set<TEntity>().IncludeAll().SingleOrDefault(i => i.Id == id);
            else
                data = context.Set<TEntity>().Find(id);

            return data;
        }

        public virtual TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            TEntity data = null;

            if (includeProperties.Length > 0)
                data = context.Set<TEntity>().IncludeProperties(includeProperties).SingleOrDefault(i => i.Id == id);
            else
                data = context.Set<TEntity>().Find(id);

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
