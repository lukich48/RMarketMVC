using AutoMapper;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Abstract;
using RMarket.WebUI.Helpers;
using RMarket.WebUI.Infrastructure.ParamEntityConverters;
using RMarket.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure.MapperProfiles
{
    public class AutoMapperUIProfile: Profile
    {
        public AutoMapperUIProfile()
        {
            CreateMap<ParamEntity, ParamEntityUI>()
                .ForMember(d => d.OriginValue, opt => opt.MapFrom(s => s.FieldValue))
                .ForMember(d => d.TypeName, opt => opt.MapFrom(s => s.FieldValue.GetType().AssemblyQualifiedName))
                .ForMember(d => d.FieldValue, opt => opt.MapFrom(s => new ParamEntityConverterHelper().ConvertToViewModel(s.FieldValue)));

            CreateMap<ParamEntityUI, ParamEntity>()
                .ForMember(d => d.FieldValue, opt => opt.MapFrom(s => new ParamEntityConverterHelper().ConvertToDomainModel(s.FieldValue, s.TypeName)));

            CreateMap<ParamSelection, ParamSelectionUI>()
                .ForMember(d => d.OriginValueMin, opt => opt.MapFrom(s => s.ValueMin))
                .ForMember(d => d.OriginValueMax, opt => opt.MapFrom(s => s.ValueMax))
                .ForMember(d => d.TypeName, opt => opt.MapFrom(s => s.ValueMin.GetType().AssemblyQualifiedName))
                .ForMember(d => d.ValueMin, opt => opt.MapFrom(s => new ParamEntityConverterHelper().ConvertToViewModel(s.ValueMin)))
                .ForMember(d => d.ValueMax, opt => opt.MapFrom(s => new ParamEntityConverterHelper().ConvertToViewModel(s.ValueMax)));

            CreateMap<ParamSelectionUI, ParamSelection>()
                .ForMember(d => d.ValueMin, opt => opt.MapFrom(s => new ParamEntityConverterHelper().ConvertToDomainModel(s.ValueMin, s.TypeName)))
                .ForMember(d => d.ValueMax, opt => opt.MapFrom(s => new ParamEntityConverterHelper().ConvertToDomainModel(s.ValueMax, s.TypeName)));

            CreateMap<HistoricalProviderSettingModel, HistoricalProviderSettingModelUI>();
            CreateMap<HistoricalProviderSettingModelUI, HistoricalProviderSettingModel>();

            CreateMap<DataProviderSettingModel, DataProviderSettingModelUI>();
            CreateMap<DataProviderSettingModelUI, DataProviderSettingModel>();

            CreateMap<OptimizationSettingModel, OptimizationSettingModelUI>();
            CreateMap<OptimizationSettingModelUI, OptimizationSettingModel>();


            CreateMap<InstanceModel, InstanceModelUI>();
            CreateMap<InstanceModelUI, InstanceModel>();

            CreateMap<SelectionModel, SelectionModelUI>();
            CreateMap<SelectionModelUI, SelectionModel>();


        }


    }
}