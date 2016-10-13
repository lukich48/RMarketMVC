using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public class SettingHelper
    {

        /// <summary>
        /// Создает параметры из сохраненных данных 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public IEnumerable<ParamEntity> GetSettingParams(IEntitySetting setting)
        {
            if (setting.EntityInfo == null)
                throw new CustomException($"settingId={setting.Id}. EntityInfo is null!");

            IEnumerable<ParamEntity> savedParams = GetSavedEntityParams(setting);
            //object entity = CreateEntityObject<object>(setting, resolver);

            //IEnumerable<ParamEntity> res = GetEntityParams<ParamEntity>(entity, savedParams);

            return savedParams;
        }

        public TEntity CreateEntityObject<TEntity>(ISettingModel setting, Resolver resolver)
            where TEntity: class
        {
            if (setting.EntityInfo == null)
                throw new CustomException($"settingId={setting.Id}. EntityInfo is null!");

            TEntity entity = (TEntity)resolver.Resolve(Type.GetType(setting.EntityInfo.TypeName));

            IEnumerable<ParamEntity> entityParams = GetEntityParams(entity, setting.EntityParams);

            ApplyParams(entity, entityParams);

            return entity;
        }

        /// <summary>
        /// Применяем сохраненные параметры
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="entityParams"></param>
        private void ApplyParams<TEntity>(TEntity entity, IEnumerable<ParamEntity> entityParams) where TEntity : class
        {
            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityAttributes(entity);
            foreach (PropertyInfo prop in arrayProp)
            {
                ParamEntity savedParam = entityParams.FirstOrDefault(p => p.FieldName == prop.Name);

                if (savedParam != null)
                {
                    prop.SetValue(entity, savedParam.FieldValue);
                }
            }
        }

        /// <summary>
        /// Получает коллекцию сохраненных параметров. Только сериализуемые поля
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        private IEnumerable<ParamEntity> GetSavedEntityParams(IEntitySetting setting)
        {
            IEnumerable<ParamEntity> strategyParams;

            if (!String.IsNullOrEmpty(setting.StrParams))
            {
                strategyParams = Serializer.Deserialize<List<ParamEntity>>(setting.StrParams); 
            }
            else
                strategyParams = new List<ParamEntity>();

            return strategyParams;
        }

        /// <summary>
        /// Создает параметры ParamBase на основании переданного объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityInfo"></param>
        /// <param name="savedParams"></param>
        /// <returns></returns>
        public IEnumerable<T> GetEntityParams<T>(IEntityInfo entityInfo, Resolver resolver, IEnumerable<T> savedParams = null)
            where T : ParamBase, new()
        {
            object entity = resolver.Resolve(Type.GetType(entityInfo.TypeName));

            return GetEntityParams(entity, savedParams);
        }

        /// <summary>
        /// Создает параметры ParamBase на основании переданного объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="savedParams"></param>
        /// <returns></returns>
        public IEnumerable<T> GetEntityParams<T>(object entity, IEnumerable<T> savedParams = null)
            where T : ParamBase, new()
        {
            if (savedParams == null)
                savedParams = new List<T>();

            List<T> res = new List<T>();

            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityAttributes(entity);

            foreach (PropertyInfo prop in arrayProp)
            {
                T savedParam = savedParams.FirstOrDefault(p => p.FieldName == prop.Name);
                if (savedParam == null)
                    savedParam = new T();

                savedParam.RepairValue(prop, entity);
                res.Add(savedParam);
            }

            return res;
        }

    }
}
