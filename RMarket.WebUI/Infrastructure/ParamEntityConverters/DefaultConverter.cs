using RMarket.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure.ParamEntityConverters
{
    public class DefaultConverter : IEntityParamConverter<object>
    {
        private Type typeValue;

        public DefaultConverter(Type typeValue)
        {
            this.typeValue = typeValue;
        }

        public string ToViewModel (object value)
        {
            return value.ToString();
        }

        public object ToDomainModel(string strValue)
        {
            return Convert.ChangeType(strValue, typeValue, CultureInfo.InvariantCulture);
        }
    }
}