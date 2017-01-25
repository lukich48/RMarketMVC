using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.Models;
using RMarket.Concrete.Optimization.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Concrete.Optimization.Helpers
{
    /// <summary>
    /// хелпер должен жить на всем протяжении жизни оптимизации
    /// </summary>
    public class GaHelper
    {
        private SelectionModel selection;
        private MyMapper mapper;
        private IList<ParamSelection> _integralParams;
        private List<ParamEntity> _otherParams;


        public GaHelper(SelectionModel selection, MyMapper mapper)
        {
            this.selection = selection;
            this.mapper = mapper;

            _integralParams = selection.SelectionParams.Where(p => p.ValueMin.IsIntegral() && (dynamic)p.ValueMax > (dynamic)p.ValueMin).ToList();
            _otherParams = selection.SelectionParams.Where(p => !_integralParams.Any(ip => ip.FieldName == p.FieldName)).Select(p => {
                var param = new ParamEntity { FieldValue = p.ValueMin };
                param.CopyObject(p);
                return param;
            }
            ).ToList();

        }
        /// <summary>
        /// Случайно генерирует начальную популяцию. Количество особей равно сумме диапазона всех параметров * на мощность
        /// </summary>
        /// <param name="multiplier">Мощность</param>
        /// <returns></returns>
        public IList<EncodedInstanceModel> CreateFirstGeneration(int multiplier = 1)
        {
            var res = new List<EncodedInstanceModel>();

            //заполняеем всеми возможными значениями
            var fieldResults = _integralParams.AsParallel()
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
                        
                        newInstance.EntityParams.AddRange(_otherParams);

                        var encodedParams = new List<EncodedEntityParam>();
                        foreach (ParamSelection paramSelection in _integralParams)
                        {
                            ParamEntity newParam = mapper.Map<ParamEntity>(paramSelection);

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
                            EncodedEntityParams = encodedParams,
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
        public IList<EncodedInstanceModel> SelectBestInstance(IDictionary<EncodedInstanceModel, decimal> fitnessResults)
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


        public IEnumerable<EncodedInstanceModel> CrossingSplit(IList<EncodedInstanceModel> instances)
        {
            var childInstances = new ConcurrentBag<EncodedInstanceModel>();

            // выбираем случайно пару значений
            var rnd = new Random();

            Parallel.For(0, instances.Count, (i) =>
            {
                var first = instances[rnd.Next(instances.Count)];
                var second = instances[rnd.Next(instances.Count)];

                if (first != second)
                {
                    EncodedInstanceModel child = CrossingParrent(first, second, rnd);
                    childInstances.Add(child);
                }
            });

            return childInstances;

        }

        internal EncodedInstanceModel CrossingParrent(EncodedInstanceModel first, EncodedInstanceModel second, Random rnd)
        {
            InstanceModel newInstance = mapper.Map<SelectionModel, InstanceModel>(selection);

            newInstance.EntityParams.AddRange(_otherParams);

            var encodedParams = new List<EncodedEntityParam>();

            foreach(var paramSelection in _integralParams)
            {
                var firstParam = first.EncodedEntityParams.Single(p=>p.FieldName == paramSelection.FieldName);
                var secondParam = second.EncodedEntityParams.Single(p=>p.FieldName == paramSelection.FieldName);

                int valueLength = firstParam.BinaryValue.Length;

                //для каждого параметра случайно находим точку кроссинговера
                int point = rnd.Next(1, valueLength - 1);

                //Скрещиваем
                var paramChild = new EncodedEntityParam
                {
                    FieldName = firstParam.FieldName,
                    BinaryValue = firstParam.BinaryValue.Substring(0, point)
                    + secondParam.BinaryValue.Substring(point, valueLength - point)
                };

                //декодируем двоичный параметр в целочисленный 
                paramChild.FieldValue = ConvertFromBinary(paramChild.BinaryValue, paramSelection.ValueMin.GetType());

                //Применяем провило приведения, если значение вышло за границы диапазона
                //todo: передать и применить правило

                encodedParams.Add(paramChild);

                ParamEntity newParam = mapper.Map<ParamEntity>(paramSelection);
                newParam.FieldValue = paramChild.FieldValue;//!!!после приведения

                newInstance.EntityParams.Add(newParam);             

            }

            return new EncodedInstanceModel
            {
                Instance = newInstance,
                EncodedEntityParams = encodedParams
            };
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

        /// <summary>
        /// конвертирует двоичное представление обратно в значение
        /// </summary>
        /// <param name="binaryValue">строка в двоичном виде</param>
        /// <param name="type">тип значения</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Если параметр не целочисленный</exception>
        private object ConvertFromBinary(string binaryValue, Type type)
        {
            object value;
            if (type == typeof(long))
                value = (long)Convert.ToInt64(binaryValue, 2);
            else if (type == typeof(int))
                value = (int)Convert.ToInt32(binaryValue, 2);
            else if (type == typeof(short))
                value = (short)Convert.ToInt16(binaryValue, 2);
            else if (type == typeof(byte))
                value = (byte)Convert.ToByte(binaryValue, 2);
            else
                throw new NotSupportedException("Метод используется только для целочисленных параметров");

            return value;
        }

    }
}
