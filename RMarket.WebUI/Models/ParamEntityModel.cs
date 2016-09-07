using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Models
{
    public class ParamEntityModel
    {
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public string FieldValue { get; set; }
        public string Description { get; set; }

        public string TypeName { get; set; }
        public object OriginValue { get; set; }


    }
}