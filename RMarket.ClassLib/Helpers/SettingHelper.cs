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

            IEnumerable<ParamEntity> savedParams = GetSavedParams<ParamEntity>(setting);
 
            return savedParams;
        }

        public TEntity CreateEntityObject<TEntity>(ISettingModel setting, Resolver resolver)
            where TEntity: class
        {
            if (setting.EntityInfo == null)
                throw new CustomException($"settingId={setting.Id}. EntityInfo is null!");

            TEntity entity = (TEntity)resolver.Resolve(Type.GetType(setting.EntityInfo.TypeName));

            IEnumerable<ParamEntity> entityParams = GetEntityParams(setting.EntityInfo, setting.EntityParams);

            ApplyParams(entity, entityParams);

            return entity;
        }

        /// <summary>
        /// Применяем сохраненные параметры
        /// </summary>
        /// <typeparam name="object"></typeparam>
        /// <param name="entity"></param>
        /// <param name="entityParams"></param>
        private void ApplyParams(object entity, IEnumerable<ParamEntity> entityParams)
        {
            foreach (ParamEntity entityParam in entityParams)
            {
                //если параметр null, значит его не было во время редактирования.
                //оставляем в таком случае дефолтовый
                if (entityParam?.FieldValue != null)
                {
                    PropertyInfo prop = entity.GetType().GetProperty(entityParam.FieldName);
                    prop.SetValue(entity, entityParam.FieldValue);
                }
            }

        }

        /// <summary>
        /// Получает коллекцию сохраненных параметров
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSavedParams<T>(IEntitySetting setting)
            where T : ParamBase, new()
        {
            IEnumerable<T> strategyParams;

            if (!String.IsNullOrEmpty(setting.StrParams))
            {
                // получаем параметры в типе json
                var savedParams = Serializer.Deserialize<List<T>>(setting.StrParams);

                // теперь нужно десериализовать каждый параметр в правильный тип
                strategyParams = GetEntityParams<T>(setting.EntityInfo, savedParams);
            }
            else
                strategyParams = new List<T>();

            return strategyParams;
        }

        /// <summary>
        /// Создает параметры ParamBase на основании типа и сохраненных данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityInfo"></param>
        /// <param name="savedParams"></param>
        /// <returns></returns>
        public IEnumerable<T> GetEntityParams<T>(IEntityInfo entityInfo, IEnumerable<T> savedParams)
            where T : ParamBase, new()
        {
            if (savedParams == null)
                savedParams = new List<T>();

            List<T> res = new List<T>();
            Type entityType = Type.GetType(entityInfo.TypeName);

            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityAttributes(entityType);

            foreach (PropertyInfo prop in arrayProp)
            {
                T savedParam = savedParams.FirstOrDefault(p => p.FieldName == prop.Name);
                if (savedParam == null)
                    savedParam = new T();

                savedParam.RepairValue(prop, entityType);
                res.Add(savedParam);
            }

            return res;
        }

        /// <summary>
        /// Создает дефолтовые параметры ParamBase на основании переданного объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IEnumerable<T> GetEntityParams<T>(object entity)
            where T : ParamBase, new()
        {
            List<T> res = new List<T>();

            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityAttributes(entity.GetType());

            foreach (PropertyInfo prop in arrayProp)
            {
                T savedParam = new T();

                savedParam.RepairValue(prop, entity);
                res.Add(savedParam);
            }

            return res;
        }

        public void RepairValues(object entity, IEnumerable<ParamBase> entityParams)
        {
            foreach (ParamBase entityParam in entityParams)
            {
                entityParam.RepairValue(entity);
            }
        }
    }
}
