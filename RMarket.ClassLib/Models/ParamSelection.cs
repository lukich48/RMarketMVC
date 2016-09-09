using Newtonsoft.Json;
using RMarket.ClassLib.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ParamSelection: ParamBase
    {
        private object _valueMin;
        private object _valueMax;

        [JsonProperty]  
        public override string FieldName { get; set; }

        [JsonProperty]
        public object ValueMin
        {
            get
            {
                return _valueMin;
            }
            set
            {
                _valueMin = GetValue(value); 
            }
        }

        [JsonProperty]
        public object ValueMax
        {
            get
            {
                return _valueMax;
            }
            set
            {
                _valueMax = GetValue(value); 
            }
        }

        public override void RepairValue(PropertyInfo prop, object entity)
        {
            ParameterAttribute attr = (ParameterAttribute)prop.GetCustomAttribute((Type)typeof(ParameterAttribute), (bool)false);

            FieldName = prop.Name;

            try
            {
                ValueMin = CastToType(ValueMin, prop.PropertyType);
            }
            catch (Exception)
            {
                ValueMin = null;
            }

            if (ValueMin == null)
                ValueMin = prop.GetValue(entity);

            try
            {
                ValueMax = CastToType(ValueMax, prop.PropertyType);
            }
            catch (Exception)
            {
                ValueMax = null;
            }

            if (ValueMax == null)
                ValueMax = prop.GetValue(entity);

            DisplayName = (attr.Name == null) ? prop.Name : attr.Name;
            Description = (attr.Description == null) ? "" : attr.Description;
            TypeName = prop.PropertyType.FullName;
        }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    List<ValidationResult> errors = new List<ValidationResult>();

        //    try
        //    {
        //        ValueMin = CastToType(ValueMin, prop.PropertyType);
        //    }
        //    catch (Exception)
        //    {
        //        errors.Add(new ValidationResult("Неверно задан тип для параметра: " + DisplayName, new List<string> { "ValueMin" }));
        //    }

        //    try
        //    {
        //        ValueMax = CastToType(ValueMax, prop.PropertyType);
        //    }
        //    catch (Exception)
        //    {
        //        errors.Add(new ValidationResult("Неверно задан тип для параметра: " + DisplayName, new List<string> { "ValueMax" }));
        //    }

        //    if (errors.Count == 0)
        //    {
        //        if (((IComparable)ValueMax).CompareTo((IComparable)ValueMin) < 0)
        //            errors.Add(new ValidationResult("Максимальный параметр не может быть меньше минимального!: " + DisplayName));
        //    }

        //    return errors;
        //}

        
    }
}
