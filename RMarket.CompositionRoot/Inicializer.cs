﻿using RMarker.Concrete.DataProviders.Infrastructure;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.Concrete.HistoricalProviders.Infrastructure;
using RMarket.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.CompositionRoot
{
    public class Inicializer
    {
        public void ApplicationStart()
        {
            //Заполнение БД
            IContextInitializer<DataProviderSetting> dataProviderInitializer = new DataProvidersContextInicializer();
            IContextInitializer<HistoricalProviderSetting> historicalProviderInitializer = new HistoricalProvidersContextInitializer();

            RMarketInitializer.DataProviderInitializer = dataProviderInitializer;
            RMarketInitializer.HistoricalProviderInitializer = historicalProviderInitializer;
        } 

    }
}
