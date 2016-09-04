﻿using AutoMapper;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.AutomapperConfigurations
{
    public class BasicConfigurarion
    {
        public MapperConfiguration CreateDefaultConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                //Instance
                cfg.CreateMap<Instance, InstanceModel>()
                .ForMember(m => m.EntityParams, opt => opt.MapFrom(d =>
                       GetEntityParamsVaried(d)));

                cfg.CreateMap<InstanceModel, Instance>()
                .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                       Serializer.Serialize(m.EntityParams)))
                 .ForMember(d => d.EntityInfo, opt => opt.Ignore())
                 .ForMember(d => d.Ticker, opt => opt.Ignore())
                 .ForMember(d => d.TimeFrame, opt => opt.Ignore())
                 .ForMember(d => d.Selection, opt => opt.Ignore());

                //Selection
                cfg.CreateMap<Selection, SelectionModel>()
                .ForMember(m => m.SelectionParams, opt => opt.MapFrom(d =>
                        GetSelectionParamsVaried(d)
                    ));

                cfg.CreateMap<SelectionModel, Selection>()
                .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                        Serializer.Serialize(m.SelectionParams)))
                .ForMember(d => d.EntityInfo, opt => opt.Ignore())
                .ForMember(d => d.Ticker, opt => opt.Ignore())
                .ForMember(d => d.TimeFrame, opt => opt.Ignore())
                .ForMember(d => d.Instances, opt => opt.Ignore());

                //DataProvider
                cfg.CreateMap<DataProviderSetting, DataProviderSettingModel>()
                .ForMember(m => m.EntityParams, opt => opt.MapFrom(d =>
                       GetEntityParamsVaried(d)
                      ));

                cfg.CreateMap<DataProviderSettingModel, DataProviderSetting>()
                .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                       Serializer.Serialize(m.EntityParams)))
                .ForMember(d => d.EntityInfo, opt => opt.Ignore());

                //HistoricalProvider
                cfg.CreateMap<HistoricalProviderSetting, HistoricalProviderSettingModel>()
                .ForMember(m => m.EntityParams, opt => opt.MapFrom(d =>
                       GetEntityParamsVaried(d)
                      ));

                cfg.CreateMap<HistoricalProviderSettingModel, HistoricalProviderSetting>()
                .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                       Serializer.Serialize(m.EntityParams)))
                .ForMember(d => d.EntityInfo, opt => opt.Ignore());
            
            });
        }

        private static IEnumerable<ParamSelection> GetSelectionParamsVaried(Selection selection)
        {
            IEnumerable<ParamSelection> res = null;

            if (selection.EntityInfo != null)
                res = StrategyHelper.GetSelectionParams(selection);
            else
                res = new List<ParamSelection>();

            return res;
        }

        private static IEnumerable<ParamEntity> GetEntityParamsVaried(IEntitySetting setting)
        {
            IEnumerable<ParamEntity> res = null;

            if(setting.EntityInfo != null)
                res = new SettingHelper().GetSettingParams(setting);
            else
                res = new List<ParamEntity>();

            return res;
        }



    }
}
