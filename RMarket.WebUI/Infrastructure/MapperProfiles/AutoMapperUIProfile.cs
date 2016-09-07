using AutoMapper;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Abstract;
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
                .ForMember(d => d.TypeName, opt => opt.MapFrom(s => s.FieldValue.GetType().FullName))
                .ForMember(d => d.FieldValue, opt => opt.MapFrom(s => SerializeFieldValue(s.FieldValue)));

            CreateMap<ParamEntityModel, ParamEntity>()
                .ForMember(d => d.FieldValue, opt => opt.MapFrom(s => DeserializeFieldValue(s.FieldValue, s.TypeName)));
        }

        /// <summary>
        /// Возвращает строковое представление на форму ввода
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public string SerializeFieldValue(object fieldValue)
        {
            IEntityParamConverter<object> converter = GetEntityParamConverter(fieldValue.GetType());

            return converter.ToViewModel(fieldValue);
        }

        /// <summary>
        /// Десериализует строковое представление в объект
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public object DeserializeFieldValue(string fieldValue, string typeName)
        {
            Type type = Type.GetType(typeName);
            IEntityParamConverter<object> converter = GetEntityParamConverter(type);

            return converter.ToDomainModel(fieldValue);
        }

        /// <summary>
        /// подбирает конвертер для отображения на форме под тип значения
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public IEntityParamConverter<object> GetEntityParamConverter(Type type)
        {
            //IEntityParamConverter converter = null;

            if (type == typeof(TimeSpan))
            {
                return new AdapterToObjectConverter<TimeSpan>(new TimeSpanConverter());
            }

            //var collectionValue = fieldValue as IEnumerable<KeyValuePair<string, string>>;
            //if(collectionValue!=null)
            //{

            //    //return
            //}

            return new ParamEntityConverters.DefaultConverter(type);

        }
    }
}