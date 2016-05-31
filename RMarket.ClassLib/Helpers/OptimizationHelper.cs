using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public static class OptimizationHelper
    {
        /// <summary>
        /// Случайно генерирует начальную популяцию. Количество особей равно сумме диапазона всех параметров * на мощность
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="multiplier">Мощность</param>
        /// <returns></returns>
        public static List<InstanceModel> CreateFirstGeneration(SelectionModel selection, int multiplier = 1)
        {
            List<InstanceModel> res = new List<InstanceModel>();

            List<ParamSelection> integralParams = selection.SelectionParams.Where(p => p.ValueMin.IsIntegral() && (dynamic)p.ValueMax > (dynamic)p.ValueMin).ToList();
            List<ParamEntity> otherParams = selection.SelectionParams.Where(p => !p.ValueMin.IsIntegral()).Select(p => {
                var param = new ParamEntity { FieldValue = p.ValueMin };
                param.CopyObject(p);
                return param;
            }
            ).ToList();

            //заполняеем всеми возможными значениями
            var fieldResults = integralParams.AsParallel()
                .Select(p => new KeyValuePair<string, IEnumerable<int>>(p.FieldName, (IEnumerable<int>)Enumerable.Range(0, (dynamic)p.ValueMax - (dynamic)p.ValueMin + 1)));

            foreach (KeyValuePair<string, IEnumerable<int>> pair in fieldResults)
            {
                for (int j = 0; j < multiplier; j++)
                {
                    foreach (int i in pair.Value)
                    {
                        InstanceModel newInstance = new InstanceModel();
                        newInstance.CopyObject(selection);
                        newInstance.SelectionId = selection.Id;
                        
                        newInstance.StrategyParams.AddRange(otherParams);

                        foreach (ParamSelection paramSelection in integralParams)
                        {
                            ParamEntity newParam = new ParamEntity();
                            newParam.CopyObject(paramSelection);
                            if (paramSelection.FieldName == pair.Key)
                            {
                                // Сохраним у значения исходный тип
                                newParam.FieldValue = Convert.ChangeType((dynamic)paramSelection.ValueMin + i, Type.GetType(paramSelection.TypeName));
                            }
                            else
                            {
                                Random rnd = new Random();
                                newParam.FieldValue = Convert.ChangeType(rnd.Next(paramSelection.ValueMin.ToIntSave(), paramSelection.ValueMax.ToIntSave()), Type.GetType(paramSelection.TypeName));
                            }

                            newInstance.StrategyParams.Add(newParam);

                        }

                        res.Add(newInstance);
                    }
                }
            }

            return res;
        }

    }
}
