using RMarket.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure.ParamEntityConverters
{
    /// <summary>
    /// класс нужен для приведения конвертеров к единому интерфейсу
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AdapterToObjectConverter<T>: IEntityParamConverter<object>
    {
        private IEntityParamConverter<T> converter;

        public AdapterToObjectConverter(IEntityParamConverter<T> converter)
        {
            this.converter = converter;
        }

        public object ToDomainModel(string strValue, Type typeValue)
        {
            return (object)converter.ToDomainModel(strValue, typeValue);
        }

        public string ToViewModel(object value)
        {
            return converter.ToViewModel((T)value);
        }
    }
}