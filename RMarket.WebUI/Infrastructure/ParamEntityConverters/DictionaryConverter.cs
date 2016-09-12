using RMarket.WebUI.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace RMarket.WebUI.Infrastructure.ParamEntityConverters
{
    public class DictionaryConverter : IEntityParamConverter<Dictionary<string,string>>
    {
        public string ToViewModel (Dictionary<string, string> value)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var pair in value)
            {
                sb.AppendLine($"{pair.Key} - {pair.Value}");
            }

            return sb.ToString();
        }

        public Dictionary<string, string> ToDomainModel(string strValue)
        {
            Dictionary<string, string> value = new Dictionary<string, string>();

            var matches = Regex.Matches(strValue, @"(\w*) ?- ?(\w*)");
            foreach(Match match in matches)
            {
                value.Add(match.Result("$1"), match.Result("$2"));
            }

            return value;
        }
    }
}