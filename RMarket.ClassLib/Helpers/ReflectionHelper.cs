using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public static class ReflectionHelper
    {
        public static Expression<Func<TEntity, object>>[] GetNavigationProperties<TEntity>()
        {
            PropertyInfo[] props = typeof(TEntity).GetProperties();

            var query = props.Where(p => IsCustomClass(p)).ToArray();
            var res = new Expression<Func<TEntity, object>>[query.Length]; 

            for (int i = 0; i<query.Length;  i++)
            {
                res[i] = GenerateMemberExpression<TEntity, object>(query[i]);
            }
       
            return res;
        }

        public static bool IsCustomClass(PropertyInfo prop)
        {
            return prop.PropertyType.IsClass && prop.PropertyType!=typeof(string);
        }

        private static Expression<Func<TModel, T>> GenerateMemberExpression<TModel, T>(PropertyInfo propertyInfo)
        {
            //var propertyInfo = typeof(TModel).GetProperty(propertyName);

            var entityParam = Expression.Parameter(typeof(TModel), "e");
            Expression columnExpr = Expression.Property(entityParam, propertyInfo);

            if (propertyInfo.PropertyType != typeof(T))
                columnExpr = Expression.Convert(columnExpr, typeof(T));

            return Expression.Lambda<Func<TModel, T>>(columnExpr, entityParam);
        }
    }
}
