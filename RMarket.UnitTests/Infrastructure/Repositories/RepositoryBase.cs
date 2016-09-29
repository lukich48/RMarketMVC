using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> : IEntityRepository<TEntity>
            where TEntity : class, IEntityData
    {
        public ICollection<TEntity> context;

        //public RepositoryBase()
        //{
        //    context = new List<TEntity>();
        //}

        public virtual IEnumerable<TEntity> Get()
        {
            return context;
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression)
        {
            IQueryable<TEntity> dataCollection = expression.Compile()(context.AsQueryable());
            return dataCollection;
        }

        public virtual IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression)
        {
            IEnumerable<TResult> dataCollection = expression.Compile()(context.AsQueryable()).ToList();

            return dataCollection;
        }

        public virtual TEntity GetById(int id, bool includeAll)
        {
            TEntity data = null;

            if (includeAll)
                data = context.AsQueryable().IncludeAll().SingleOrDefault(i => i.Id == id);
            else
                data = context.SingleOrDefault(i => i.Id == id);

            return data;
        }

        public virtual TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            TEntity data = null;

            if (includeProperties.Length > 0)
                data = context.AsQueryable().IncludeProperties(includeProperties).SingleOrDefault(i => i.Id == id);
            else
                data = context.SingleOrDefault(i => i.Id == id);

            return data;
        }

        public virtual void Save(TEntity data)
        {
            if (data.Id == 0)
            {
                context.Add(data);
            }
            else
            {
                context.Remove(context.SingleOrDefault(i => i.Id == data.Id));
                context.Add(data);

            }

        }

        public virtual void Remove(int id)
        {
            TEntity data = context.SingleOrDefault(i => i.Id == id);
            context.Remove(data);
        }

        public void AddRange(IEnumerable<TEntity> data)
        {
            foreach (TEntity dataItem in data)
            {
                context.Add(dataItem);
            }
        }

        public void RemoveRange(IEnumerable<TEntity> data)
        {
            foreach (TEntity dataItem in data)
            {
                context.Remove(dataItem);
            }
        }
    }
}
