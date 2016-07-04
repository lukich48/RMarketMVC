using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using System.Reflection;
using RMarket.ClassLib.Entities;
using System.Globalization;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.EntityModels;

namespace RMarket.ClassLib.Helpers
{
    public static class StrategyHelper
    {

        /// <summary>
        /// Подписываем на ассинхронную версию событиия формирования свечи
        /// </summary>
        /// <param name="strategy"></param>
        public static void SubscriptionToEventAsync(IStrategy strategy)
        {
            MemberInfo[] arrayProp = strategy.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property,
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null);

            foreach (MemberInfo prop in arrayProp)
            {
                IIndicator curInd = null;
                if (prop.MemberType == MemberTypes.Property)
                {
                    curInd = (prop as PropertyInfo).GetValue(strategy) as IIndicator;
                }
                else if (prop.MemberType == MemberTypes.Field)
                {
                    curInd = (prop as FieldInfo).GetValue(strategy) as IIndicator;
                }

                if (curInd != null)
                {
                    //Подписываем Async
                    strategy.Instr.CreatedCandle -= curInd.AddValue;
                    strategy.Instr.CreatedCandleAsync += (sender) => { curInd.AddValue(sender, null); };
                }
            }
        }

        /// <summary>
        /// Создает новый объект стратегии и применяет сохраненные параметры.
        /// </summary>
        /// <param name="strategyInfo"></param>
        /// <param name="strategyParams"></param>
        /// <returns></returns>
        public static IStrategy CreateStrategy(InstanceModel instance)
        {
            if (instance.StrategyInfo == null)
                throw new CustomException($"instanceId={instance.Id}. StrategyInfo is null!");

            IStrategy strategy = (IStrategy)ReflectionHelper.CreateEntity(instance.StrategyInfo);

            //Применяем сохраненные параметры
            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityProps(strategy);
            foreach (PropertyInfo prop in arrayProp)
            {
                ParamEntity savedParam = instance.StrategyParams.FirstOrDefault(p => p.FieldName == prop.Name);

                if (savedParam != null)
                {
                    prop.SetValue(strategy, savedParam.FieldValue);
                }
            }

            strategy.Orders = new List<Order>();

            return strategy;
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

            object entity = ReflectionHelper.CreateEntity(entityInfo);

            IEnumerable<PropertyInfo> arrayProp = ReflectionHelper.GetEntityProps(entity);

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

        /// <summary>
        /// Создает параметры из объекта Стратегии и сохраненных данных 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IEnumerable<ParamEntity> GetStrategyParams(Instance instance)
        {
            if (instance.StrategyInfo == null)
                throw new CustomException($"instanceId={instance.Id}. StrategyInfo is null!");

            IEnumerable<ParamEntity> savedParams = GetSavedStrategyParams(instance);

            List<ParamEntity> res = StrategyHelper.GetEntityParams<ParamEntity>(instance.StrategyInfo, savedParams);

            return res;
        }

        /// <summary>
        /// Создает параметры из объекта Селекция и сохраненных данных 
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        public static IEnumerable<ParamSelection> GetStrategyParams(Selection selection)
        {
            IEnumerable<ParamSelection> savedParams = GetSavedStrategyParams(selection);

            List<ParamSelection> res = StrategyHelper.GetEntityParams<ParamSelection>(selection.StrategyInfo, savedParams);

            return res;
        }

        /// <summary>
        /// Извлекает индикаторы из стратегии
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static Dictionary<string, IIndicator> ExtractIndicatorsInStrategy(IStrategy strategy)
        {
            //Найти все индикаторы с атрибутом [DisplayChartAttribute] (могут быть полями или свойствами) и заполнить listIndicators
            Dictionary<string, IIndicator> listIndicators = new Dictionary<string, IIndicator>();
            MemberInfo[] arrayProp = strategy.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property,
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                new MemberFilter(ReflectionHelper.FilterAttributes), new DisplayChartAttribute());

            foreach (MemberInfo prop in arrayProp)
            {
                DisplayChartAttribute attr = (DisplayChartAttribute)prop.GetCustomAttribute(typeof(DisplayChartAttribute), false);
                string dispName = (attr.Name == null) ? prop.Name : attr.Name; //Свойство Name из атрибута - в легенду

                IIndicator curInd = null;
                if (prop.MemberType == MemberTypes.Property)
                {
                    curInd = (prop as PropertyInfo).GetValue(strategy) as IIndicator;
                }
                else if (prop.MemberType == MemberTypes.Field)
                {
                    curInd = (prop as FieldInfo).GetValue(strategy) as IIndicator;
                }

                if (curInd != null)
                    listIndicators.Add(dispName, curInd);
            }

            return listIndicators;
        }

        #region Служебные приватные методы

        /// <summary>
        /// Получает коллекцию сохраненных параметров. Только сериализуемые поля
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static IEnumerable<ParamEntity> GetSavedStrategyParams(Instance instance)
        {
            List<ParamEntity> strategyParams;

            if (!String.IsNullOrEmpty(instance.StrParams))
            {
                strategyParams = Serializer.Deserialize<List<ParamEntity>>(instance.StrParams);
            }
            else
                strategyParams = new List<ParamEntity>();

            return strategyParams;
        }

        /// <summary>
        /// Получает коллекцию сохраненных параметров. Только сериализуемые поля
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        private static IEnumerable<ParamSelection> GetSavedStrategyParams(Selection selection)
        {
            IEnumerable<ParamSelection> strategyParams;

            if (!String.IsNullOrEmpty(selection.StrParams))
            {
                strategyParams = Serializer.Deserialize<List<ParamSelection>>(selection.StrParams);
            }
            else
                strategyParams = new List<ParamSelection>();

            return strategyParams;
        }

        #endregion

    }
}
