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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Concrete.Optimization
{
    public class GaSimple: IOptimization
    {
        private readonly ICandleRepository candleRepository;
        private readonly IResolver resolver;

        public MyMapper Mapper { get; set; }

        [Parameter(Description = "Множитель популяции. 1 - нормальная популяция")]
        public int GenerationPower { get; set; } = 1;

        [Parameter(Name ="Корректировочная функция", Description ="Вызывается при получении параметра превышающего заданный диапазон")]
        public CorrectFunction CorrectFunction { get; set; } = CorrectFunction.ToBordersPartialy;

        [Parameter(Name = "Процент корректировки", Description = "Для функции \"К границам с превышением\"")]
        public int CorrectPersent { get; set; } = 12;

        public GaSimple(ICandleRepository candleRepository, IResolver resolver)
        {
            this.candleRepository = candleRepository;
            this.resolver = resolver;

        }

        public List<InstanceModel> Start(SelectionModel selection, DateTime dateFrom, DateTime dateTo)
        {
            List<InstanceModel> res = new List<InstanceModel>();
            GaHelper helper = new GaHelper(selection, Mapper);

            //Выбрка начальной популяции
            IList<EncodedInstanceModel> firstGen = helper.CreateFirstGeneration(GenerationPower);

            //1. Определяем значение фитнес-функции
            IDictionary<EncodedInstanceModel, decimal> fitnessResults = RunStrategies(dateFrom, dateTo, firstGen);

            //2. Отбираем лучшие (репродукция)
            IList<EncodedInstanceModel> best = helper.SelectBestInstance(fitnessResults);

            //3. Кроссинговер
            IEnumerable<EncodedInstanceModel>  children = helper.CrossingSplit(best);

            //4. Мутация
            //todo: сделать мутациюIEnumerable<EncodedInstanceModel>

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
                //todo: выделить в метод запуск стратегии
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

    public enum CorrectFunction
    {
        [Display(Name = "К границам", Description = "все вышедшие варианты приводит к граничным значениям")]
        ToBorders,
        [Display(Name = "К границам с превышением", Description = "приводит к границам, если значение больше допустимого превышения")]
        ToBordersPartialy
    }

}
