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

        public static SelectList GetStrategyInfoList(IStrategyInfoRepository strategyInfoRepository)
        {
            return new SelectList(strategyInfoRepository.Get(), "Id", "Name");
        }

        public static SelectList GetInstanceList(ClassLib.Abstract.IService.IInstanceService instanceService)
        {
            return new SelectList(instanceService.Get(), "Id", "Name");
        }

        public static SelectList GetConnectorInfoList(IConnectorInfoRepository connectorInfoRepository)
        {
            return new SelectList(connectorInfoRepository.Get(), "Id", "Name");
        }

        public static SelectList GetSettingList(ISettingService settingService, SettingType settingType)
        {
            return new SelectList(settingService.Get(T=>T.Where(s=>s.SettingType == settingType)), "Id", "Name");
        }

    }
}