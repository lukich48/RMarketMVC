using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;

namespace RMarket.ClassLib.Abstract.IRepository
{
    public interface IEntityRepository<TEntity>: IEntityRepository
    {
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression);
        IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression);
        TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(int id, bool includeAll);
        void Save(TEntity data);
        void Remove(int id);
        void AddRange(IEnumerable<TEntity> data);
        void RemoveRange(IEnumerable<TEntity> data);

    }
}