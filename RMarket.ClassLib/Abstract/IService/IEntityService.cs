using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract.IService
{
    public interface IEntityService<TEntity,TModel>: IEntityService
    {
        IEnumerable<TModel> Get();
        IEnumerable<TModel> Get(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> expression);
        IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<TEntity>, IQueryable<TResult>>> expression);
        TModel GetById(int id, bool includeAll = false);
        TModel GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        void Save(TModel model);
        void Remove(int id);
    }
}
