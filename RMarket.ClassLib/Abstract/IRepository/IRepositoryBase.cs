using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;

namespace RMarket.ClassLib.Abstract
{
    public interface IRepositoryBase<TEntity>
    {
        RMarketContext Context { get; set; }

        IEnumerable<TEntity> Get1();
        IEnumerable<TEntity> Get1(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression);
        IEnumerable<TResult> Get1<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression);
        TEntity GetById1(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById1(int id, bool includeAll);
        void Save(TEntity data);
    }
}