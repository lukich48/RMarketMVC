using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Infrastructure;
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
        /// Создает параметры из объекта Селекция и сохраненных данных 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public IEnumerable<ParamEntity> GetSettingParams(IEntitySetting setting)
        {
            if (setting.EntityInfo == null)
                throw new CustomException($"settingId={setting.Id}. EntityInfo is null!");

            IEnumerable<ParamEntity> savedParams = GetSavedEntityParams(setting);

            IEnumerable<ParamEntity> res = GetEntityParams<ParamEntity>(setting.EntityInfo, savedParams);

            return res;
        }

        public IDataProvider CreateDataProvider(DataProviderSettingModel setting)
        {
            if (setting.EntityInfo == null)
                throw new CustomException($"settingId={setting.Id}. EntityInfo is null!");

            IDataProvider dataProvider = (IDataProvider)ReflectionHelper.CreateEntity(setting.EntityInfo);

            IEnumerable<ParamEntity> strategyParams = GetEntityParams<ParamEntity>(setting.EntityInfo, setting.EntityParams);

            //Применяем сохраненные параметры
            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityAttributes(dataProvider);
            foreach (PropertyInfo prop in arrayProp)
            {
                ParamEntity savedParam = strategyParams.FirstOrDefault(p => p.FieldName == prop.Name);

                if (savedParam != null)
                {
                    prop.SetValue(dataProvider, savedParam.FieldValue);
                }
            }

            return dataProvider;
        }

        public TEntity CreateEntityObject<TEntity>(ISettingModel setting)
            where TEntity: class
        {
            if (setting.EntityInfo == null)
                throw new CustomException($"settingId={setting.Id}. EntityInfo is null!");

            TEntity dataProvider = (TEntity)ReflectionHelper.CreateEntity(setting.EntityInfo);

            IEnumerable<ParamEntity> strategyParams = GetEntityParams<ParamEntity>(setting.EntityInfo, setting.EntityParams);

            //Применяем сохраненные параметры
            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityAttributes(dataProvider);
            foreach (PropertyInfo prop in arrayProp)
            {
                ParamEntity savedParam = strategyParams.FirstOrDefault(p => p.FieldName == prop.Name);

                if (savedParam != null)
                {
                    prop.SetValue(dataProvider, savedParam.FieldValue);
                }
            }

            return dataProvider;
        }

        /// <summary>
        /// Получает коллекцию сохраненных параметров. Только сериализуемые поля
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public IEnumerable<ParamEntity> GetSavedEntityParams(IEntitySetting setting)
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
        public IEnumerable<T> GetEntityParams<T>(IEntityInfo entityInfo, IEnumerable<T> savedParams = null)
            where T : ParamBase, new()
        {
            if (savedParams == null)
                savedParams = new List<T>();

            List<T> res = new List<T>();

            object entity = ReflectionHelper.CreateEntity(entityInfo);

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
