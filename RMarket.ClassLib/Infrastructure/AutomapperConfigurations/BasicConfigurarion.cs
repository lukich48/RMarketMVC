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
            });
        }

        public static IEnumerable<ParamEntity> GetStrategyParamsVaried(Instance instance)
        {
            IEnumerable<ParamEntity> res = null;

            if (instance.StrategyInfo != null)
                res = StrategyHelper.GetStrategyParams(instance);
            else
                res = new List<ParamEntity>();

            return res;
        }

        public static IEnumerable<ParamSelection> GetSelectionParamsVaried(Selection instance)
        {
            IEnumerable<ParamSelection> res = null;

            if (instance.StrategyInfo != null)
                res = StrategyHelper.GetStrategyParams(instance);
            else
                res = new List<ParamSelection>();

            return res;
        }


    }
}
