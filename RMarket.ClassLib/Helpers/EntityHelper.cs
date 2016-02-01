using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public class EntityHelper
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
        /// Создает параметры ParamBase на основании переданного объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityInfo"></param>
        /// <param name="savedParams"></param>
        /// <returns></returns>
        public static List<T> GetEntityParams<T>(IEntityInfo entityInfo, IEnumerable<T> savedParams = null)
            where T : ParamBase, new()
        {
            if (savedParams == null)
                savedParams = new List<T>();

            List<T> res = new List<T>();

            object entity = CreateEntity(entityInfo);

            MemberInfo[] arrayProp = GetEntityProps(entity);

            foreach (MemberInfo prop in arrayProp)
            {
                T savedParam = savedParams.FirstOrDefault(p => p.FieldName == prop.Name);
                if (savedParam == null)
                    savedParam = new T();

                savedParam.RepairValue(prop, entity);
                res.Add(savedParam);
            }

            return res;
        }

        /// <summary>
        /// Извлекает из объекта поля с атрибутом ParameterAttribute
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static MemberInfo[] GetEntityProps(object entity)
        {
            MemberInfo[] arrayProp = entity.GetType().FindMembers(MemberTypes.Field, //!!! Добавить для свойств
                    BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    new MemberFilter(FilterAttributes), new ParameterAttribute());

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


    }
}
