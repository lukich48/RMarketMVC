﻿using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Infrastructure.AmbientContext;
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
            object entity = Resolver.Current.Resolve(Type.GetType(entityInfo.TypeName));

            return entity;
        }

        /// <summary>
        /// Извлекает из объекта поля с атрибутом ParameterAttribute
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetEntityAttributes(object entity)
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

        public static Expression<Func<TEntity, object>>[] GerNavigationPropertiesExpression<TEntity>()
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

        public static IEnumerable<PropertyInfo> GetNavigationProperties<TEntity>()
        {
            var expressions = GerNavigationPropertiesExpression<TEntity>();
            return expressions.Select(e => e.GetProperty());
        }

        public static bool IsCustomClass(PropertyInfo prop)
        {
            return prop.PropertyType.IsClass && prop.PropertyType!=typeof(string);
        }

        public static PropertyInfo GetProperty(this LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentOutOfRangeException("expression", "Expected a property/field access expression, not " + expression);
            }

            PropertyInfo property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentOutOfRangeException("expression", "Expected a property access expression, not " + expression);

            return property;
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
