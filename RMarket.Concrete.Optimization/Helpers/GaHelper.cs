using RMarket.ClassLib.Dto.Optimization;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Concrete.Optimization.Helpers
{
    public class GaHelper
    {
        /// <summary>
        /// Случайно генерирует начальную популяцию. Количество особей равно сумме диапазона всех параметров * на мощность
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="multiplier">Мощность</param>
        /// <returns></returns>
        public IEnumerable<EncodedInstanceModel> CreateFirstGeneration(SelectionModel selection, int multiplier = 1)
        {
            var res = new List<EncodedInstanceModel>();

            List<ParamSelection> integralParams = selection.SelectionParams.Where(p => p.ValueMin.IsIntegral() && (dynamic)p.ValueMax > (dynamic)p.ValueMin).ToList();
            List<ParamEntity> otherParams = selection.SelectionParams.Where(p => !integralParams.Any(ip=>ip.FieldName==p.FieldName)).Select(p => {
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
                        
                        newInstance.EntityParams.AddRange(otherParams);

                        var encodedParams = new List<EncodedEntityParam>();
                        foreach (ParamSelection paramSelection in integralParams)
                        {
                            ParamEntity newParam = new ParamEntity();
                            newParam.CopyObject(paramSelection);
                            if (paramSelection.FieldName == pair.Key)
                            {
                                // Сохраним у значения исходный тип
                                newParam.FieldValue = Convert.ChangeType((dynamic)paramSelection.ValueMin + i, paramSelection.ValueMin.GetType());
                            }
                            else
                            {
                                Random rnd = new Random();
                                newParam.FieldValue = Convert.ChangeType(rnd.Next(paramSelection.ValueMin.ToIntSave(), paramSelection.ValueMax.ToIntSave()), paramSelection.ValueMin.GetType());
                            }

                            newInstance.EntityParams.Add(newParam);

                            //получаем двоичное представление
                            encodedParams.Add(new EncodedEntityParam
                            {
                                FieldName = newParam.FieldName,
                                BinaryValue = ConvertToBinary(newParam.FieldValue)
                            });

                        }

                        EncodedInstanceModel encodedInstance = new EncodedInstanceModel
                        {
                            Instance = newInstance,
                            EncodedEntityParams = encodedParams
                        };


                        res.Add(encodedInstance);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// выбрать лучшие по значению фитнес-функции
        /// </summary>
        /// <param name="fitnessResults"></param>
        /// <returns></returns>
        public IEnumerable<EncodedInstanceModel> SelectBestInstance(IDictionary<EncodedInstanceModel, decimal> fitnessResults)
        {
            var listResult = new List<EncodedInstanceModel>();
            foreach (var fitnesResult in fitnessResults)
            {
                int amount = (int)Math.Round(fitnesResult.Value / fitnessResults.Average(r => r.Value));

                if (amount > 0)
                    listResult.AddRange(Enumerable.Repeat(fitnesResult.Key, amount));
            }

            return listResult;
        }

        /// <summary>
        /// Кодирует в двоичный вид
        /// </summary>
        /// <param name="entityParam"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Если параметр не целочисленный</exception>
        private string ConvertToBinary(object value)
        {

            string bits;
            if (value is long)
                bits = Convert.ToString((long)value, 2);
            else if (value is int)
                bits = Convert.ToString((int)value, 2);
            else if (value is short)
                bits = Convert.ToString((short)value, 2);
            else if (value is byte)
                bits = Convert.ToString((byte)value, 2);
            else
                throw new NotSupportedException("Метод используется только для целочисленных параметров");

            return bits;
        }

    }
}
