using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    public interface IRepositoryBase<TEntity,TModel>
    {
        IEnumerable<TModel> Get();
        IEnumerable<TModel> Get(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression);
        IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression);
        TModel GetById(int id, bool includeAll = false);
        TModel GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        int Save(TModel instance);
    }
}
