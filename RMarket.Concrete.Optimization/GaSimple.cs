using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Dto.Optimization;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.Managers;
using RMarket.ClassLib.Models;
using RMarket.Concrete.Optimization.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Concrete.Optimization
{
    public class GaSimple
    {
        private readonly ICandleRepository candleRepository;
        private readonly Resolver resolver;

        [Parameter(Description = "Множитель популяции. 1 - нормальная популяция")]
        public int GenerationPower { get; set; }

        public GaSimple(ICandleRepository candleRepository, Resolver resolver)
        {
            this.candleRepository = candleRepository;
            this.resolver = resolver;

            GenerationPower = 1;
        }

        public List<InstanceModel> Start(SelectionModel selection, DateTime dateFrom, DateTime dateTo)
        {
            List<InstanceModel> res = new List<InstanceModel>();
            GaHelper helper = new GaHelper();

            //Выбрка начальной популяции
            IEnumerable<EncodedInstanceModel> firstGen = helper.CreateFirstGeneration(selection, GenerationPower);

            //1. Определяем значение фитнес-функции
            IDictionary<EncodedInstanceModel, decimal> fitnessResults = RunStrategies(dateFrom, dateTo, firstGen);

            //2. Отбираем лучшие (репродукция)
            IEnumerable<EncodedInstanceModel> best = helper.SelectBestInstance(fitnessResults);

            //3. Кодируем парамеры

            //4. Кроссинговер

            //5. Мутация

            //6. Декодируем параметры

            //Снова 1

            return res;
        }

        /// <summary>
        /// Запускает все экземпляры на выполнение
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="firstGen"></param>
        /// <returns>Результат - это значение фитнес-функции</returns>
        private IDictionary<EncodedInstanceModel, decimal> RunStrategies(DateTime dateFrom, DateTime dateTo, IEnumerable<EncodedInstanceModel> firstGen)
        {
            var fitnessResults = new ConcurrentDictionary<EncodedInstanceModel, decimal>();

            Parallel.ForEach(firstGen, (EncodedInstanceModel encodedInstance) =>
            {
                InstanceModel instance = encodedInstance.Instance;

                //получаем стратегию 
                IStrategy strategy = new SettingHelper().CreateEntityObject<IStrategy>(instance, resolver);

                //устанавливаем остальные свойства
                Instrument instr = new Instrument(instance.Ticker, instance.TimeFrame);

                Portfolio portf = new Portfolio
                {
                    Balance = instance.Balance,
                    Rent = instance.Rent,
                    Slippage = instance.Slippage
                };

                IManager manager = new TesterManager(candleRepository, strategy, instr, portf);

                manager.DateFrom = dateFrom;
                manager.DateTo = dateTo;

                //Стартуем стратегию
                manager.StartStrategy();

                //Вычисляем значение фитнес-функции (пока просто суммарный профит)
                //todo: Модуль определния значения фитнес-функции
                decimal profit = strategy.Orders.Sum(o => o.Profit);

                fitnessResults[encodedInstance] = profit;

            });
            return fitnessResults;
        }
    }
}
