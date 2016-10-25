using AutoMapper;
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

namespace RMarket.ClassLib.MapperProfiles
{
    public class AutoMapperDomainProfile: Profile
    {
        public AutoMapperDomainProfile()
        {
            //Instance
            CreateMap<Instance, InstanceModel>()
            .ForMember(m => m.EntityParams, opt => opt.MapFrom(d =>
                   GetEntityParamsVaried(d)));

            CreateMap<InstanceModel, Instance>()
            .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                   Serializer.Serialize(m.EntityParams)))
             .ForMember(d => d.EntityInfo, opt => opt.Ignore())
             .ForMember(d => d.Ticker, opt => opt.Ignore())
             .ForMember(d => d.TimeFrame, opt => opt.Ignore())
             .ForMember(d => d.Selection, opt => opt.Ignore());

            //Selection
            CreateMap<Selection, SelectionModel>()
            .ForMember(m => m.SelectionParams, opt => opt.MapFrom(d =>
                    GetSelectionParamsVaried(d)
                ));

            CreateMap<SelectionModel, Selection>()
            .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                    Serializer.Serialize(m.SelectionParams)))
            .ForMember(d => d.EntityInfo, opt => opt.Ignore())
            .ForMember(d => d.Ticker, opt => opt.Ignore())
            .ForMember(d => d.TimeFrame, opt => opt.Ignore())
            .ForMember(d => d.Instances, opt => opt.Ignore());

            //DataProvider
            CreateMap<DataProviderSetting, DataProviderSettingModel>()
            .ForMember(m => m.EntityParams, opt => opt.MapFrom(d =>
                   GetEntityParamsVaried(d)
                  ));

            CreateMap<DataProviderSettingModel, DataProviderSetting>()
            .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                   Serializer.Serialize(m.EntityParams)))
            .ForMember(d => d.EntityInfo, opt => opt.Ignore());

            //HistoricalProvider
            CreateMap<HistoricalProviderSetting, HistoricalProviderSettingModel>()
            .ForMember(m => m.EntityParams, opt => opt.MapFrom(d =>
                   GetEntityParamsVaried(d)
                  ));

            CreateMap<HistoricalProviderSettingModel, HistoricalProviderSetting>()
            .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                   Serializer.Serialize(m.EntityParams)))
            .ForMember(d => d.EntityInfo, opt => opt.Ignore());

            //Optimization
            CreateMap<OptimizationSetting, OptimizationSettingModel>()
            .ForMember(m => m.EntityParams, opt => opt.MapFrom(d =>
               GetEntityParamsVaried(d)
              ));

            CreateMap<OptimizationSettingModel, OptimizationSetting>()
            .ForMember(d => d.StrParams, opt => opt.MapFrom(m =>
                   Serializer.Serialize(m.EntityParams)))
            .ForMember(d => d.EntityInfo, opt => opt.Ignore());

            //Selection Instance
            CreateMap<SelectionModel, InstanceModel>()
                .ForMember(d=>d.SelectionId, opt=>opt.MapFrom(s=>s.Id));
            CreateMap<InstanceModel, SelectionModel>();

            CreateMap<ParamSelection, ParamEntity>()
                .ForMember(d=>d.FieldValue, opt=>opt.MapFrom(s=>s.ValueMin));
            CreateMap<ParamEntity, ParamSelection>()
                .ForMember(d => d.ValueMin, opt => opt.MapFrom(s => s.FieldValue))
                .ForMember(d => d.ValueMax, opt => opt.MapFrom(s => s.FieldValue));



        }

        private static IEnumerable<ParamSelection> GetSelectionParamsVaried(Selection selection)
        {
            IEnumerable<ParamSelection> res = null;

            if (selection.EntityInfo != null)
                res = new SettingHelper().GetSelectionParams(selection);
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
