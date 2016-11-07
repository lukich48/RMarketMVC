using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.Infrastructure.AmbientContext;
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
        private readonly IEntityRepository<TEntity> repository;

        public MyMapper Mapper { get; set; }

        public EntityServiceBase(IEntityRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public virtual IEnumerable<TModel> Get()
        {
            return MyMapper.Current.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(repository.Get());
        }

        public virtual IEnumerable<TModel> Get(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression)
        {
            IEnumerable<TEntity> dataCollection = repository.Get(expression);
            return MyMapper.Current.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(dataCollection);
        }

        public virtual IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression)
        {
            return repository.Get(expression);
        }

        public virtual TModel GetById(int id, bool includeAll)
        {
            TEntity data = repository.GetById(id, includeAll);
            TModel model = MyMapper.Current.Map<TEntity, TModel>(data);

            return model;
        }

        public virtual TModel GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            TEntity data = repository.GetById(id, includeProperties);
            TModel instance = MyMapper.Current.Map<TEntity, TModel>(data);

            return instance;
        }

        public virtual void Save(TModel model)
        {
            TEntity data = MyMapper.Current.Map<TModel, TEntity>(model);
            repository.Save(data);
        }

        public virtual void Remove(int id)
        {
            repository.Remove(id);
        }

    }
}
