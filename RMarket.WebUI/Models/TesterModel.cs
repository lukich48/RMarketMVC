using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Infrastructure.HighChart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace RMarket.WebUI.Models
{
    public class TesterModel
    {
        [Display(Name = "Вариант")]
        public int InstanceId { get; set; }
        [Display(Name = "Коннектор")]
        public int ConnectorInfoId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

    }

    public class TestResult
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public InstanceModel Instance { get; set; }
        public IStrategy Strategy { get; set; }
        public IManager Manager { get; set; }
        //Ключ - имя индикатора
        public Dictionary<string, IIndicator> IndicatorsDict { get; set; }

        #region //////////////////Сбор данных для графика

        /// <summary>
        /// Загрузка крайних n свечек
        /// </summary>
        /// <param name="resultId"></param>
        /// <param name="maxCount"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        public TestResultJson GetDataJsonInit(int maxCount, string way = "right")
        {
            var res = GetCandlesOutside(maxCount, way);

            return res;
        }

        /// <summary>
        /// загрузка свечей после определенной даты вперед-назад
        /// </summary>
        /// <param name="resultId"></param>
        /// <param name="lastDate"></param>
        /// <param name="maxCount"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        public TestResultJson GetDataJsonSlice(DateTime lastDate, int maxCount, string way = "right")
        {
            TestResultJson res;

            res = GetCandlesSlice(lastDate, maxCount, way);

            //Если меньше maxCount, то берем скраю
            if (res.Candles.Count< maxCount)
                res = GetCandlesOutside(maxCount, way);

            return res;
        }

        /// <summary>
        /// подгрузка оставшихся значений
        /// </summary>
        /// <param name="resultId"></param>
        /// <param name="lastDate"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public TestResultJson GetDataJsonAdd(DateTime lastDate, int maxCount)
        {
            List<Candle> candles = new List<Candle>();
            List<Order> orders = new List<Order>();
            Dictionary<string, List<IndicatorValue>> indicators = new Dictionary<string, List<IndicatorValue>>();

            if (Strategy.Instr.Candles.Count == 0)
            {
                return new TestResultJson();
            }

            candles = Strategy.Instr.Candles.Where(c => c.DateOpen > lastDate).OrderBy(c => c.DateOpen).Take(maxCount).ToList();
            DateTime dateMin = candles[0].DateOpen;
            orders = Strategy.Orders.Where(o => o.DateOpen >= dateMin).OrderBy(o => o.DateOpen).ToList();

            indicators = new Dictionary<string, List<IndicatorValue>>();
            foreach (KeyValuePair<string, IIndicator> indPair in IndicatorsDict)
            {
                foreach (KeyValuePair<string, IndicatorResult> resultPair in indPair.Value.Results)
                {
                    List<IndicatorValue> indRes = resultPair.Value.Values.Where(c => c.DateOpen > lastDate).OrderBy(c => c.DateOpen).Take(maxCount).ToList();
                    indicators.Add(indPair.Key + "_" + resultPair.Key, indRes);
                }
            }

            var res = Create_TestResultJson(candles, orders, indicators);

            return res;
        }


        #region Приватные методы

        /// <summary>
        /// Отбирает крайние свечки
        /// </summary>
        /// <param name="maxCount"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        private TestResultJson GetCandlesOutside(int maxCount, string way = "right")
        {
            List<Candle> candles = new List<Candle>();
            List<Order> orders = new List<Order>();
            Dictionary<string, List<IndicatorValue>> indicators = new Dictionary<string, List<IndicatorValue>>();

            if (Strategy.Instr.Candles.Count == 0)
            {
                return new TestResultJson();
            }

            if (way == "right")
            {
                candles = Strategy.Instr.Candles.Where((c, i) => i < maxCount).OrderBy(c => c.DateOpen).ToList();
                DateTime dateMin = candles[0].DateOpen;
                orders = Strategy.Orders.Where(o => o.DateOpen >= dateMin).OrderBy(o => o.DateOpen).ToList();

                foreach (KeyValuePair<string, IIndicator> indPair in IndicatorsDict)
                {
                    foreach (KeyValuePair<string, IndicatorResult> resultPair in indPair.Value.Results)
                    {
                        List<IndicatorValue> indRes = resultPair.Value.Values.Where((c, i) => i < maxCount).OrderBy(c => c.DateOpen).ToList();
                        indicators.Add(indPair.Key + "_" + resultPair.Key, indRes);
                    }
                }
            }
            else
            {
                candles = Strategy.Instr.Candles.Where((c, i) => i >= (Strategy.Instr.Candles.Count - maxCount)).OrderBy(c => c.DateOpen).ToList();
                DateTime dateMax = candles.Last().DateOpen;
                orders = Strategy.Orders.Where(o => o.DateOpen <= dateMax).OrderBy(o => o.DateOpen).ToList();

                foreach (KeyValuePair<string, IIndicator> indPair in IndicatorsDict)
                {
                    foreach (KeyValuePair<string, IndicatorResult> resultPair in indPair.Value.Results)
                    {
                        List<IndicatorValue> indRes = resultPair.Value.Values.Where((c, i) => i >= (Strategy.Instr.Candles.Count - maxCount)).OrderBy(c => c.DateOpen).ToList();
                        indicators.Add(indPair.Key + "_" + resultPair.Key, indRes);
                    }
                }
            }

            var res = Create_TestResultJson(candles, orders, indicators);

            return res;
        }

        /// <summary>
        /// Отбирает свечки от указанной даты
        /// </summary>
        /// <param name="lastDate"></param>
        /// <param name="maxCount"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        private TestResultJson GetCandlesSlice(DateTime lastDate, int maxCount, string way = "right")
        {
            List<Candle> candles = new List<Candle>();
            List<Order> orders = new List<Order>();
            Dictionary<string, List<IndicatorValue>> indicators = new Dictionary<string, List<IndicatorValue>>();

            if (Strategy.Instr.Candles.Count == 0)
            {
                return new TestResultJson();
            }

            if (way == "right")
            {
                candles = Strategy.Instr.Candles.Where(c => c.DateOpen > lastDate).OrderBy(c => c.DateOpen).Take(maxCount).ToList();
                DateTime dateMax = candles.Last().DateOpen;
                orders = Strategy.Orders.Where(o => o.DateOpen > lastDate && o.DateOpen <= dateMax).OrderBy(o => o.DateOpen).ToList();

                foreach (KeyValuePair<string, IIndicator> indPair in IndicatorsDict)
                {
                    foreach (KeyValuePair<string, IndicatorResult> resultPair in indPair.Value.Results)
                    {
                        List<IndicatorValue> indRes = resultPair.Value.Values.Where(c => c.DateOpen > lastDate).OrderBy(c => c.DateOpen).Take(maxCount).ToList();
                        indicators.Add(indPair.Key + "_" + resultPair.Key, indRes);
                    }
                }
            }
            else
            {
                candles = Strategy.Instr.Candles.Where(c => c.DateOpen < lastDate).Take(maxCount).OrderBy(c => c.DateOpen).ToList();
                DateTime dateMin = candles[0].DateOpen;
                orders = Strategy.Orders.Where(o => o.DateOpen >= dateMin && o.DateOpen < lastDate).OrderBy(o => o.DateOpen).ToList();

                foreach (KeyValuePair<string, IIndicator> indPair in IndicatorsDict)
                {
                    foreach (KeyValuePair<string, IndicatorResult> resultPair in indPair.Value.Results)
                    {
                        List<IndicatorValue> indRes = resultPair.Value.Values.Where(c => c.DateOpen < lastDate).Take(maxCount).OrderBy(c => c.DateOpen).ToList();
                        indicators.Add(indPair.Key + "_" + resultPair.Key, indRes);
                    }
                }
            }

            var res = Create_TestResultJson(candles, orders, indicators);

            return res;
        }

        private TestResultJson Create_TestResultJson(List<Candle> candles, List<Order> orders, Dictionary<string, List<IndicatorValue>> indicators)
        {
            List<object[]> candleData = new List<object[]>();
            foreach (Candle curCandle in candles)
            {
                object[] data = new object[5];
                data[0] = curCandle.DateOpen.MillisecondUTC();
                data[1] = curCandle.OpenPrice;
                data[2] = curCandle.HighPrice;
                data[3] = curCandle.LowPrice;
                data[4] = curCandle.ClosePrice;

                candleData.Add(data);
            }

            //Статус процесса выполнения стратегии 1-Стратегия запущена. 2-стратегия запущена обновлять не требуется. 3-выбрана самая ранняя свеча
            int status = 0;

            if (candles[0] == Strategy.Instr.Candles.Last())
                status = 3;
            else if (candles.Last() == Strategy.Instr.Candles[0])
            {
                if (Manager.IsStarted)
                    status = 1;
                else
                    status = 2;
            }

            //Ордера. нужно определить начальный баланс
            decimal startBalance = Manager.Portf.Balance + Strategy.Orders.Where(o => o.DateOpen < orders[0].DateOpen).Sum(o => o.Profit);
            List<SeriesData> orderData = new List<SeriesData>();
            orderData.Add(new SeriesData { x = candles[0].DateOpen.MillisecondUTC(), y = startBalance }); //крайнее значение
            foreach (Order curOrder in orders)
            {
                startBalance += curOrder.Profit;
                SeriesData data = new SeriesData();
                data.x = curOrder.DateOpen.MillisecondUTC();
                data.y = startBalance;

                orderData.Add(data);
            }
            orderData.Add(new SeriesData { x = candles.Last().DateOpen.MillisecondUTC(), y = startBalance }); //крайнее значение

            //Индикаторы. Для каждой линии свой массив
            List<SeriesOption> indicatorList = new List<SeriesOption>();
            foreach (KeyValuePair<string, List<IndicatorValue>> curInd in indicators)
            {
                SeriesOption indicatorJson = new SeriesOption();
                indicatorList.Add(indicatorJson);

                indicatorJson.name = curInd.Key;

                List<SeriesData> dataList = new List<SeriesData>();
                foreach (IndicatorValue value in curInd.Value)
                {
                    SeriesData data = new SeriesData();
                    data.x = value.DateOpen.MillisecondUTC();
                    data.y = value.Value;

                    dataList.Add(data);
                }
                indicatorJson.data = dataList;

            }

            //Сделки для каждой сделки свой график
            List<SeriesOption> profitList = new List<SeriesOption>();
            foreach (Order curOrder in orders)
            {
                List<SeriesData> dataList = new List<SeriesData>();

                dataList.Add( new SeriesData
                {
                    x = curOrder.DateOpen.MillisecondUTC(),
                    y = curOrder.PriceOpen
                });

                if (curOrder.PriceClose != 0)
                {
                    dataList.Add(new SeriesData
                    {
                        x = curOrder.DateClose?.MillisecondUTC(),
                        y = curOrder.PriceClose,
                        dataLabels = new DataLabel
                        {
                            format =curOrder.Profit.ToString()
                        }
                    });
                }

                profitList.Add(new SeriesOption
                {
                    name = curOrder.Profit.ToString(),
                    data = dataList,
                    lineWidth = 1,
                    color = curOrder.Profit>0?"green":"red",
                    marker = new MarkerOption
                    {
                        radius = 3
                    }
            });
            }

            var res = new TestResultJson
            {
                Status = status,
                Candles = candleData,
                Orders = new SeriesOption { data = orderData },
                Indicators = indicatorList,
                ProfitList = profitList,
            };

            return res;

        }

        #endregion
        #endregion

    }


    public class TestResultJson
    {
        public int Status { get; set; }
        public List<object[]> Candles { get; set; }
        public SeriesOption Orders { get; set; }
        public List<SeriesOption> Indicators { get; set; }
        public List<SeriesOption> ProfitList { get; set; }

    }



}