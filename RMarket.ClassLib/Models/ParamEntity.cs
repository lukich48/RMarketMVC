using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using RMarket.ClassLib.Entities;
using Newtonsoft.Json;
using System.Reflection;

namespace RMarket.ClassLib.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ParamEntity :ParamBase
    {
        private object _fieldValue;

        [JsonProperty]
        public override string FieldName { get; set; }
        [JsonProperty]
        public object FieldValue { get
            {
                return _fieldValue;
            }
            set
            {
                _fieldValue = GetValue(value);
            }
        } 

        public ParamEntity()
        {
            FillHelper();
        }

        private void FillHelper(string DisplayName ="", string Description = "")
        {
            this.DisplayName = DisplayName;
            this.Description = Description;
        }

        public override void RepairValue(PropertyInfo prop, object entity)
        {
            ParameterAttribute attr = (ParameterAttribute)prop.GetCustomAttribute(typeof(ParameterAttribute), false);

            FieldName = prop.Name;
            TypeName = prop.PropertyType.FullName;

            try
            {
                FieldValue = CastToType(FieldValue, prop.PropertyType);
            }
            catch (Exception)
            {
                FieldValue = null;
            }

            if (FieldValue == null)
                FieldValue = prop.GetValue(entity);

            DisplayName = (attr.Name == null) ? prop.Name : attr.Name;
            Description = (attr.Description == null) ? "": attr.Description;
        }

    }

}
