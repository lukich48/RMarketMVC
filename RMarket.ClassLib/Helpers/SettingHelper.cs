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
    public static class SettingHelper
    {
        public static IConnectorInfoRepository connectorInfoRepository;//!!! = CurrentRepository.ConnectorInfoRepository;

        /// <summary>
        /// Создает параметры из объекта Селекция и сохраненных данных 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static IEnumerable<ParamEntity> GetSettingParams(Setting setting)
        {
            IEnumerable<ParamEntity> savedParams = GetSavedStrategyParams(setting);
            IEntityInfo entityInfo = GetEntityInfo(setting.SettingType, setting.EntityInfoId);

            List<ParamEntity> res = StrategyHelper.GetEntityParams<ParamEntity>(entityInfo, savedParams).ToList();

            return res;
        }

        /// <summary>
        /// Возвращает объект по типу настройки и идентификатору. Генерирует исключение, если неверно задан settingType
        /// </summary>
        /// <param name="settingType"></param>
        /// <param name="entityInfoId"></param>
        /// <returns></returns>
        public static IEntityInfo GetEntityInfo (SettingType settingType, int entityInfoId)
        {
            IEntityInfo entityInfo = null;
            switch (settingType)
            {
                case SettingType.ConnectorInfo:
                    entityInfo = connectorInfoRepository.GetById(entityInfoId);
                    break;
                default:
                    throw new CustomException(String.Format("Не найден тип настройки!", settingType));                    
            }

            return entityInfo;
        }

        ///// <summary>
        ///// Находит провайдер по умолчанию для стратегии
        ///// </summary>
        ///// <param name="strategyInfo"></param>
        ///// <returns></returns>
        //public static IDataProvider CreateDataProvider(StrategyInfo strategyInfo)
        //{
        //    //Найдем актуальную настройку для стратегии
        //    Setting setting = settingRepository.Get(T=>T.Where(s => s.SettingType == SettingType.ConnectorInfo && s.StrategyInfoId == strategyInfo.Id).OrderByDescending(s => s.Priority)).FirstOrDefault();

        //    return CreateDataProvider(setting);
        //}

        /// <summary>
        /// Находит провайдер по умолчанию для стратегии
        /// </summary>
        /// <param name="connectorInfo"></param>
        /// <returns></returns>
        public static IDataProvider CreateDataProvider(Setting setting)
        {
            ConnectorInfo connectorInfo = connectorInfoRepository.GetById(setting.EntityInfoId);

            IDataProvider dataProvider = (IDataProvider)ReflectionHelper.CreateEntity(connectorInfo);

            IEnumerable<ParamEntity> savedParams = Serializer.Deserialize<IEnumerable<ParamEntity>>(setting.StrParams);

            IEnumerable<ParamEntity> strategyParams = StrategyHelper.GetEntityParams<ParamEntity>(connectorInfo, savedParams);

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
        /// Находит провайдер по умолчанию для стратегии
        /// </summary>
        /// <param name="connectorInfo"></param>
        /// <returns></returns>
        public static IDataProvider CreateDataProvider(SettingModel setting)
        {
            ConnectorInfo connectorInfo = connectorInfoRepository.GetById(setting.EntityInfoId);

            IDataProvider dataProvider = (IDataProvider)ReflectionHelper.CreateEntity(connectorInfo);

            IEnumerable<ParamEntity> strategyParams = StrategyHelper.GetEntityParams<ParamEntity>(connectorInfo, setting.EntityParams);

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


        //////////////////////////Private methods

        /// <summary>
        /// Получает коллекцию сохраненных параметров. Только сериализуемые поля
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        private static IEnumerable<ParamEntity> GetSavedStrategyParams(Setting setting)
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


    }
}
