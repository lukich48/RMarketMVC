﻿using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
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
        public static IConnectorInfoRepository connectorInfoRepository = CurrentRepository.ConnectorInfoRepository;
        public static ISettingRepository settingRepository = CurrentRepository.SettingRepository;

        /// <summary>
        /// Возвращает объект по типу настройки и идентификатору. Генерирует исключение, если неверно задан typeSetting
        /// </summary>
        /// <param name="typeSetting"></param>
        /// <param name="entityInfoId"></param>
        /// <returns></returns>
        public static IEntityInfo GetEntityInfo (SettingType typeSetting, int entityInfoId)
        {
            IEntityInfo entityInfo = null;
            switch (typeSetting)
            {
                case SettingType.ConnectorInfo:
                    entityInfo = connectorInfoRepository.Find(entityInfoId);
                    break;
                default:
                    throw new CustomException(String.Format("Не найден тип настройки!", typeSetting));                    
            }

            return entityInfo;
        }

        /// <summary>
        /// Находит провайдер по умолчанию для стратегии
        /// </summary>
        /// <param name="strategyInfo"></param>
        /// <returns></returns>
        public static IDataProvider CreateDataProvider(StrategyInfo strategyInfo)
        {
            //Найдем актуальную настройку для стратегии
            Setting setting = settingRepository.Settings.Where(s => s.TypeSetting == SettingType.ConnectorInfo && s.StrategyInfoId == strategyInfo.Id).OrderByDescending(s => s.Priority).FirstOrDefault();

            if (setting == null)
            {
                setting = settingRepository.Settings.Where(s => s.TypeSetting == SettingType.ConnectorInfo && s.StrategyInfoId == null).OrderByDescending(s => s.Priority).FirstOrDefault();
            }

            ConnectorInfo connectorInfo = connectorInfoRepository.Find(setting.EntityInfoId);

            IDataProvider dataProvider =  (IDataProvider)ReflectionHelper.CreateEntity(connectorInfo);

            IEnumerable<ParamEntity> savedParams = Serializer.Deserialize<IEnumerable<ParamEntity>>(setting.StrParams);

            IEnumerable<ParamEntity> strategyParams = StrategyHelper.GetEntityParams<ParamEntity>(connectorInfo, savedParams);

            //Применяем сохраненные параметры
            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityProps(dataProvider);
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
    }
}
