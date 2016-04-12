using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Optimization
{
    public class GASimple
    {
        ISelectionRepository selectionRepository = CurrentRepository.SelectionRepository;

        public List<InstanceModel> Start(int selectionId)
        {
            SelectionModel selection = selectionRepository.FindModel(selectionId);

            List<InstanceModel> res = new List<InstanceModel>();

            //Выбрка начальной популяции !!! Мощность популяции в настройки
            List<InstanceModel> first = OptimizationHelper.CreateFirstGeneration(selection,1);
            
            //1. Определяем значение фитнес-функции

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
