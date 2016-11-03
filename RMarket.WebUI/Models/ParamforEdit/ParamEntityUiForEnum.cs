using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Models.ParamforEdit
{
    public class ParamEntityUiForEnum: ParamEntityEditForObject
    {
        public IEnumerable<SelectListItem> ValuesSelectList { get; set; }
        //public IEnumerable<string> DescriptionCollection { get; set; }
    }
}