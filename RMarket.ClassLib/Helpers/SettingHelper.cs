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
        public IEnumerable<ParamEntity> GetSettingParams(DataProvider setting)
        {
            if (setting.ConnectorInfo == null)
                throw new CustomException($"settingId={setting.Id}. EntityInfo is null!");

            IEnumerable<ParamEntity> savedParams = GetSavedStrategyParams(setting);

            List<ParamEntity> res = StrategyHelper.GetEntityParams<ParamEntity>(setting.ConnectorInfo, savedParams).ToList();

            return res;
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
        public IDataProvider CreateDataProvider(SettingModel setting)
        {
            if (setting.ConnectorInfo == null)
                throw new CustomException($"settingId={setting.Id}. EntityInfo is null!");

            IDataProvider dataProvider = (IDataProvider)ReflectionHelper.CreateEntity(setting.ConnectorInfo);

            IEnumerable<ParamEntity> strategyParams = StrategyHelper.GetEntityParams<ParamEntity>(setting.ConnectorInfo, setting.EntityParams);

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
        private IEnumerable<ParamEntity> GetSavedStrategyParams(DataProvider setting)
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
