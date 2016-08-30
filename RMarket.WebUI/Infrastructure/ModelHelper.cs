using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RMarket.ClassLib.Abstract;
using System.Web.Mvc;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.WebUI.Infrastructure
{
    public static class ModelHelper
    {
        public static SelectList GetTickerList(ITickerRepository tickerRepository)
        {
            return new SelectList(tickerRepository.Get(), "Id", "Name");
        }

        public static SelectList GetTimeFrameList(ITimeFrameRepository timeFrameRepository)
        {
            return new SelectList(timeFrameRepository.Get(), "Id", "Name");
        }

        public static SelectList GetInstanceList(ClassLib.Abstract.IService.IInstanceService instanceService)
        {
            return new SelectList(instanceService.Get(), "Id", "Name");
        }

        public static SelectList GetStrategyInfoList(IEntityInfoRepository entityInfoRepository)
        {
            return new SelectList(entityInfoRepository.Get(T => T.Where(e => e.EntityType == EntityType.StrategyInfo)), "Id", "Name");
        }

        public static SelectList GetDataProviderInfoList(IEntityInfoRepository entityInfoRepository)
        {
            return new SelectList(entityInfoRepository.Get(T=>T.Where(e=>e.EntityType== EntityType.DataProviderInfo)), "Id", "Name");
        }

        public static SelectList GetHistoricalProviderInfoList(IEntityInfoRepository entityInfoRepository)
        {
            return new SelectList(entityInfoRepository.Get(T => T.Where(e => e.EntityType == EntityType.HistoricalProviderInfo)), "Id", "Name");
        }

        public static SelectList GetDataProviderList(IDataProviderSettingService dataProviderService)
        {
            return new SelectList(dataProviderService.Get(), "Id", "Name");
        }

    }
}