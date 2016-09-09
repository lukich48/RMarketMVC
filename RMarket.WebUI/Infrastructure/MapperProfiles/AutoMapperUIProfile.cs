using AutoMapper;
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
            CreateMap<ParamEntity, ParamEntityModel>()
                .ForMember(d => d.OriginValue, opt => opt.MapFrom(s => s.FieldValue))
                .ForMember(d => d.TypeName, opt => opt.MapFrom(s => s.FieldValue.GetType().AssemblyQualifiedName))
                .ForMember(d => d.FieldValue, opt => opt.MapFrom(s => new ParamEntityConverterHelper().ConvertToViewModel(s.FieldValue)));

            CreateMap<ParamEntityModel, ParamEntity>()
                .ForMember(d => d.FieldValue, opt => opt.MapFrom(s => new ParamEntityConverterHelper().ConvertToDomainModel(s.FieldValue, s.TypeName)));
        }


    }
}