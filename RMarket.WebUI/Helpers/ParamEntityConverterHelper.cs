using RMarket.WebUI.Abstract;
using RMarket.WebUI.Infrastructure.ParamEntityConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Helpers
{
    public class ParamEntityConverterHelper
    {
        /// <summary>
        /// Возвращает строковое представление на форму ввода
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertToViewModel(object value)
        {
            IEntityParamConverter<object> converter = GetEntityParamConverter(value.GetType());

            return converter.ToViewModel(value);
        }

        /// <summary>
        /// Десериализует строковое представление в объект
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public object ConvertToDomainModel(string strValue, string typeName)
        {
            Type type = Type.GetType(typeName);

            return ConvertToDomainModel(strValue, type);
        }

        public object ConvertToDomainModel(string strValue, Type type)
        {
            IEntityParamConverter<object> converter = GetEntityParamConverter(type);

            return converter.ToDomainModel(strValue);
        }


        /// <summary>
        /// подбирает конвертер для отображения на форме под тип значения
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        private IEntityParamConverter<object> GetEntityParamConverter(Type type)
        {

            if (type == typeof(TimeSpan))
            {
                return new AdapterToObjectConverter<TimeSpan>(new TimeSpanConverter());
            }
            else if(type == typeof(Dictionary<string,string>))
            {
                return new AdapterToObjectConverter<Dictionary<string, string>>(new DictionaryConverter());
            }

            return new DefaultConverter(type);

        }

    }
}