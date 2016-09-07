using RMarket.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure.ParamEntityConverters
{
    public class TimeSpanConverter : IEntityParamConverter<TimeSpan>
    {
        public string ToViewModel (TimeSpan value)
        {
            return value.ToString();
        }

        public TimeSpan ToDomainModel(string strValue)
        {
            return TimeSpan.Parse(strValue);
        }
    }
}