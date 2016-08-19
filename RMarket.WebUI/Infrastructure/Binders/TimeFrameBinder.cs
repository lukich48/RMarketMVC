using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Infrastructure.Binders
{
    public class TimeFrameBinder: IModelBinder
    {
        private ITimeFrameRepository timeFrameRepository;

        public TimeFrameBinder(ITimeFrameRepository timeFrameRepository)
        {
            this.timeFrameRepository = timeFrameRepository;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            TimeFrame timeFrame = null;

            // Получаем поставщик значений
            IValueProvider valueProvider = bindingContext.ValueProvider;

            // получаем данные по одному полю
            ValueProviderResult vpr = null;
            if (valueProvider.GetValue("TimeFrameId") != null)
                vpr = valueProvider.GetValue("TimeFrameId");
            else if (valueProvider.GetValue("TimeFrame.Id") != null)
                vpr = valueProvider.GetValue("TimeFrame.Id");

            if (vpr != null)
            {
                int timeFrameId = (int)vpr.ConvertTo(typeof(int));
                if(timeFrameId!=0)
                    timeFrame = timeFrameRepository.GetById(timeFrameId);

            }

            return timeFrame;
        }
    }
}