using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
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
        /// <summary>
        /// Создает новый объект из строкового представления типа
        /// </summary>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        public static object CreateEntity(IEntityInfo entityInfo)
        {
            object entity = Activator.CreateInstance(Type.GetType(entityInfo.TypeName));

            return entity;
        }

        /// <summary>
        /// Извлекает из объекта поля с атрибутом ParameterAttribute
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetEntityProps(object entity)
        {
            PropertyInfo[] arrayProp = entity.GetType().FindMembers(MemberTypes.Property,
                    BindingFlags.Instance | BindingFlags.Public,
                    new MemberFilter(FilterAttributes), new ParameterAttribute()).Cast<PropertyInfo>().ToArray();

            return arrayProp;
        }

        /// <summary>
        /// служебный метод. Используется где-то в рефлексии. Служит для отбора полей и свойств
        /// </summary>
        /// <param name="objMemberInfo"></param>
        /// <param name="objSearch"></param>
        /// <returns></returns>
        public static bool FilterAttributes(MemberInfo objMemberInfo, Object objSearch)
        {
            return objMemberInfo.IsDefined(objSearch.GetType(), false);
        }

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
