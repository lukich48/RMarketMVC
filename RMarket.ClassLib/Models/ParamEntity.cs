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
using RMarket.ClassLib.Helpers;

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
                _fieldValue = value;//GetValue(value);
            }
        }

        public override void RepairValue(PropertyInfo prop, Type entityType)
        {
            ParameterAttribute attr = (ParameterAttribute)prop.GetCustomAttribute(typeof(ParameterAttribute), false);

            FieldName = prop.Name;

            if (FieldValue != null &&
                FieldValue.GetType() != prop.PropertyType &&
                FieldValue.GetType() != typeof(string))
            {
                try
                {
                    //желательно сразу string
                    FieldValue = Serializer.Deserialize(FieldValue.ToString(), prop.PropertyType);
                }
                catch (Exception)
                {
                    FieldValue = null;
                }

            }

            DisplayName = (attr.Name == null) ? prop.Name : attr.Name;
            Description = (attr.Description == null) ? "" : attr.Description;
        }

        public override void RepairValue(PropertyInfo prop, object entity)
        {
            RepairValue(prop, entity.GetType());

            if (FieldValue == null)
                FieldValue = prop.GetValue(entity);
        }

        public override void RepairValue(object entity)
        {
            if (FieldValue == null)
            {
                PropertyInfo prop = entity.GetType().GetProperty(FieldName);
                FieldValue = prop.GetValue(entity);

            }
        }


    }

}
