using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Services
{
    public abstract class EntityServiceBase<TEntity, TModel> : IEntityService<TEntity, TModel>
    {
        private readonly IRepositoryBase<TEntity> repository;
        public EntityServiceBase(IRepositoryBase<TEntity> repository)
        {
            this.repository = repository;
        }

        public virtual IEnumerable<TModel> Get()
        {
            return Current.Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(repository.Get1());
        }

        public virtual IEnumerable<TModel> Get(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression)
        {
            IEnumerable<TEntity> dataCollection = repository.Get1(expression);
            return Current.Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(dataCollection);
        }

        public virtual IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression)
        {
            return repository.Get1(expression);
        }

        public virtual TModel GetById(int id, bool includeAll)
        {
            TEntity data = repository.GetById1(id, includeAll);
            TModel instance = Current.Mapper.Map<TEntity, TModel>(data);

            return instance;
        }

        public virtual TModel GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            TEntity data = repository.GetById1(id, includeProperties);
            TModel instance = Current.Mapper.Map<TEntity, TModel>(data);

            return instance;
        }

        public virtual void Save(TModel model)
        {
            TEntity data = Current.Mapper.Map<TModel, TEntity>(model);
            repository.Save(data);
        }

    }
}
