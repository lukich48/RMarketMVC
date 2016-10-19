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
using RMarket.ClassLib.Infrastructure.AmbientContext;

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

    }
}
