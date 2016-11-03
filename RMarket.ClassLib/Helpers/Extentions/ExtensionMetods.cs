using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers.Extentions
{
    public static class ExtensionMetods
    {
        public static double MillisecondUTC(this DateTime curDate)
        {
            DateTime date1970 = new DateTime(1970, 1, 1);
            TimeSpan ts = curDate - date1970;
            return ts.TotalMilliseconds;
        }

        public static bool IsIntegral(this object obj)
        {
            bool res = false;
            Type type = obj.GetType();

            if (
                type == typeof(byte)
                || type == typeof(short)
                || type == typeof(int)
                || type == typeof(long)
                )
            {
                res = true;
            }

            return res;
        }

        public static int ToIntSave(this object obj)
        {
            int res = 0;
            bool finish = false;
            try
            {
                res = (int)obj;
                finish = true;
            }
            catch (Exception) { }

            if (!finish)
            {
                try
                {
                    res = Convert.ToInt32(obj);
                }
                catch (Exception) { }
            }

            return res;

        }

        /// <summary>
        /// Копирует значение полей и свойств
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="newObj"></param>
        /// <param name="copiedObj">Объект, с котрого копируем</param>
        /// <param name="excludeFields">объект с полями, которые нужно исключить из копирования</param>
        /// <param name="includeFields">объект с полями, которые нужно скопировать</param>
        public static void CopyObject<T1, T2>(this T1 newObj, T2 copiedObj, Func<T1, object> excludeFields = null, Func<T1, object> includeFields = null)
        {
            PropertyInfo[] propsObj = copiedObj.GetType().GetProperties();
            PropertyInfo[] propsNew = newObj.GetType().GetProperties();

            PropertyInfo[] propsExclude = null;
            if (excludeFields != null)
            {
                object excludeObject = excludeFields(newObj);
                propsExclude = excludeObject.GetType().GetProperties();
            }

            PropertyInfo[] propsInclude = null;
            if (includeFields != null)
            {
                object includeObject = includeFields(newObj);
                propsInclude = includeFields.GetType().GetProperties();
            }
            foreach (PropertyInfo prop in propsObj)
            {
                if (propsInclude != null)
                {
                    if (!propsInclude.Any(p => p.Name == prop.Name))
                        continue;
                }

                if (propsExclude != null)
                {
                    if (propsExclude.Any(p => p.Name == prop.Name))
                        continue;
                }

                PropertyInfo newProp = Array.Find(propsNew, p => p.Name == prop.Name && p.PropertyType == prop.PropertyType);
                if (newProp != null)
                {
                    newProp.SetValue(newObj, prop.GetValue(copiedObj));
                }
            }

            FieldInfo[] fieldsObj = copiedObj.GetType().GetFields();
            FieldInfo[] fieldsNew = newObj.GetType().GetFields();

            foreach (FieldInfo field in fieldsObj)
            {
                FieldInfo newField = Array.Find(fieldsNew, p => p.Name == field.Name && p.FieldType == field.FieldType);
                if (newField != null)
                {
                    newField.SetValue(newObj, field.GetValue(copiedObj));
                }
            }
        }

        public static IQueryable<TEntity>IncludeProperties<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (includeProperties!=null)
                query = (includeProperties.Aggregate(query, (current, include) => current.Include(include)));

            return query;
        }

        public static IQueryable<TEntity> IncludeAll<TEntity>(this IQueryable<TEntity> query)
        {
            IQueryable<TEntity> res = query;

            Expression<Func<TEntity, object>>[] includeProperties = ReflectionHelper.GerNavigationPropertiesExpression<TEntity>();
  
            return query.IncludeProperties(includeProperties);
        }

        ///Вывод информации из атрибута
        public static string Description(this Enum value)
        {
            Type type = value.GetType();

            List<string> res = new List<string>();
            var arrValue = value.ToString().Split(',').Select(v => v.Trim());
            foreach (string strValue in arrValue)
            {
                MemberInfo[] memberInfo = type.GetMember(strValue);
                if (memberInfo != null && memberInfo.Length > 0)
                {
                    object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                    if (attrs != null && attrs.Length > 0 && attrs.Where(t => t.GetType() == typeof(DisplayAttribute)).FirstOrDefault() != null)
                    {
                        res.Add(((DisplayAttribute)attrs.Where(t => t.GetType() == typeof(DisplayAttribute)).FirstOrDefault()).Description);
                    }
                    else
                        res.Add(strValue);
                }
                else
                    res.Add(strValue);
            }

            return res.Aggregate((s, v) => s + ", " + v);
        }

        ///Вывод информации из атрибута
        public static string Name(this Enum value)
        {
            Type type = value.GetType();

            List<string> res = new List<string>();
            var arrValue = value.ToString().Split(',').Select(v => v.Trim());
            foreach (string strValue in arrValue)
            {
                MemberInfo[] memberInfo = type.GetMember(strValue);
                if (memberInfo != null && memberInfo.Length > 0)
                {
                    object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                    if (attrs != null && attrs.Length > 0 && attrs.Where(t => t.GetType() == typeof(DisplayAttribute)).FirstOrDefault() != null)
                    {
                        res.Add(((DisplayAttribute)attrs.Where(t => t.GetType() == typeof(DisplayAttribute)).FirstOrDefault()).Name);
                    }
                    else
                        res.Add(strValue);
                }
                else
                    res.Add(strValue);
            }

            return res.Aggregate((s, v) => s + ", " + v);
        }


    }
}
