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
    public class TickerBinder : IModelBinder
    {
        private ITickerRepository tickerRepository;

        public TickerBinder(ITickerRepository tickerRepository)
        {
            this.tickerRepository = tickerRepository;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Ticker ticker = null;

            // Получаем поставщик значений
            IValueProvider valueProvider = bindingContext.ValueProvider;

            // получаем данные по одному полю
            ValueProviderResult vpr = null;
            if(valueProvider.GetValue("TickerId")!=null)
                vpr = valueProvider.GetValue("TickerId");
            else if(valueProvider.GetValue("Ticker.Id") != null)
                vpr = valueProvider.GetValue("Ticker.Id");

            if (vpr != null)
            {
                int tickerId = (int)vpr.ConvertTo(typeof(int));
                if (tickerId !=0)
                    ticker = tickerRepository.Find(tickerId);

            }

            return ticker;
        }
    }
}