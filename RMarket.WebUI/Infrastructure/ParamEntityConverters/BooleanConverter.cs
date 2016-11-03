using RMarket.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure.ParamEntityConverters
{
    public class BooleanConverter: IEntityParamConverter<bool>
    {
        public string ToViewModel(bool value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public bool ToDomainModel(string strValue, Type typeValue)
        {
            return bool.Parse(strValue);
        }
    }
}