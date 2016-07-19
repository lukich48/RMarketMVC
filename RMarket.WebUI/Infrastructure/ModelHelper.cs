using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RMarket.ClassLib.Abstract;
using System.Web.Mvc;
using RMarket.ClassLib.Entities;

namespace RMarket.WebUI.Infrastructure
{
    public static class ModelHelper
    {
        public static SelectList GetTickerList(ITickerRepository tickerRepository)
        {
            return new SelectList(tickerRepository.Tickers, "Id", "Name");
        }

        public static SelectList GetTimeFrameList(ITimeFrameRepository timeFrameRepository)
        {
            return new SelectList(timeFrameRepository.TimeFrames, "Id", "Name");
        }

        public static SelectList GetStrategyInfoList(IStrategyInfoRepository strategyInfoRepository)
        {
            return new SelectList(strategyInfoRepository.StrategyInfoes, "Id", "Name");
        }

        public static SelectList GetInstanceList(IInstanceRepository instanceRepository)
        {
            return new SelectList(instanceRepository.Get(), "Id", "Name");
        }

        public static SelectList GetConnectorInfoList(IConnectorInfoRepository connectorInfoRepository)
        {
            return new SelectList(connectorInfoRepository.ConnectorInfoes, "Id", "Name");
        }

        public static SelectList GetSettingList(ISettingRepository settingRepository, SettingType settingType)
        {
            return new SelectList(settingRepository.Settings.Where(s=>s.SettingType == settingType), "Id", "Name");
        }

    }
}