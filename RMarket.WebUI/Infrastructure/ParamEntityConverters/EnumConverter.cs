using RMarket.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure.ParamEntityConverters
{
    public class EnumConverter: IEntityParamConverter<Enum>
    {
        public string ToViewModel(Enum value)
        {
            return value.ToString("F");
        }

        public Enum ToDomainModel(string strValue, Type typeValue)
        {
            return (Enum)Enum.Parse(typeValue, strValue);
        }
    }
}