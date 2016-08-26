using AutoMapper;
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
                .ForMember(m => m.StrategyParams, opt => opt.MapFrom(d =>
                       GetStrategyParamsVaried(d)));

                cfg.CreateMap<InstanceModel, Instance>()
                .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                       Serializer.Serialize(m.StrategyParams)))
                 .ForMember(d => d.StrategyInfo, opt => opt.Ignore())
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
                .ForMember(d => d.StrategyInfo, opt => opt.Ignore())
                .ForMember(d => d.Ticker, opt => opt.Ignore())
                .ForMember(d => d.TimeFrame, opt => opt.Ignore())
                .ForMember(d => d.Instances, opt => opt.Ignore());

                //Setting
                cfg.CreateMap<DataProvider, DataProviderModel>()
                .ForMember(m => m.EntityParams, opt => opt.MapFrom(d =>
                       GetEntityParamsVaried(d)
                      ));

                cfg.CreateMap<DataProviderModel, DataProvider>()
                .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                       Serializer.Serialize(m.EntityParams)))
                .ForMember(d => d.DataProviderInfo, opt => opt.Ignore());
            });
        }

        private static IEnumerable<ParamEntity> GetStrategyParamsVaried(Instance instance)
        {
            IEnumerable<ParamEntity> res = null;

            if (instance.StrategyInfo != null)
                res = StrategyHelper.GetStrategyParams(instance);
            else
                res = new List<ParamEntity>();

            return res;
        }

        private static IEnumerable<ParamSelection> GetSelectionParamsVaried(Selection selection)
        {
            IEnumerable<ParamSelection> res = null;

            if (selection.StrategyInfo != null)
                res = StrategyHelper.GetStrategyParams(selection);
            else
                res = new List<ParamSelection>();

            return res;
        }

        private static IEnumerable<ParamEntity> GetEntityParamsVaried(DataProvider setting)
        {
            IEnumerable<ParamEntity> res = null;

            if(setting.DataProviderInfo != null)
                res = new SettingHelper().GetSettingParams(setting);
            else
                res = new List<ParamEntity>();

            return res;
        }



    }
}
