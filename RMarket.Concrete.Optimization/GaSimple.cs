using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Managers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.Concrete.Optimization
{
    public class GaSimple
    {
        private readonly ISelectionService selectionService;

        [Parameter(Description = "Множитель популяции. 1 - нормальная популяция")]
        public int GenerationPower { get; set; }

        public GaSimple(ISelectionService selectionService)
        {
            this.selectionService = selectionService;

            GenerationPower = 1;
        }

        public List<InstanceModel> Start(int selectionId, DateTime dateFrom, DateTime dateTo)
        {
            SelectionModel selection = selectionService.GetById(selectionId, s=>s.EntityInfo);

            List<InstanceModel> res = new List<InstanceModel>();

            //Выбрка начальной популяции !!! Мощность популяции в настройки
            List<InstanceModel> first = OptimizationHelper.CreateFirstGeneration(selection, GenerationPower);

            //1. Определяем значение фитнес-функции
            Dictionary<InstanceModel, IStrategy> listResult = new Dictionary<InstanceModel, IStrategy>();
            foreach (InstanceModel instance in first)
            {
                //получаем стратегию 
                IStrategy strategy = StrategyHelper.CreateStrategy(instance);
                
                //устанавливаем остальные свойства
                Instrument instr = new Instrument(instance.Ticker, instance.TimeFrame);

                Portfolio portf = new Portfolio
                {
                    Balance = instance.Balance,
                    Rent = instance.Rent,
                    Slippage = instance.Slippage
                };

                IManager manager = new TesterManager(strategy, instr, portf);

                manager.DateFrom = dateFrom;
                manager.DateTo = dateTo;

                //Стартуем стратегию
                manager.StartStrategy();

                listResult[instance] = strategy;
                
            }

            //2. Отбираем лучшие

            //3. Кодируем парамеры

            //4. Кроссинговер

            //5. Мутация

            //6. Декодируем параметры

            //Снова 1

            return res;
        }

    }
}
