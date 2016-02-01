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
    public class ParamSelection: ParamBase, IValidatableObject
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

        public override void RepairValue(MemberInfo prop, object entity)
        {
            ParameterAttribute attr = (ParameterAttribute)prop.GetCustomAttribute(typeof(ParameterAttribute), false);

            FieldInfo curField = prop as FieldInfo;

            FieldName = curField.Name;

            try
            {
                ValueMin = Convert.ChangeType(ValueMin, curField.FieldType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                ValueMin = null;
            }

            if (ValueMin == null)
                ValueMin = curField.GetValue(entity);

            try
            {
                ValueMax = Convert.ChangeType(ValueMax, curField.FieldType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                ValueMax = null;
            }

            if (ValueMax == null)
                ValueMax = curField.GetValue(entity);

            DisplayName = (attr.Name == null) ? curField.Name : attr.Name;
            Description = (attr.Description == null) ? "" : attr.Description;
            TypeName = curField.FieldType.FullName;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if(!ValidateParam(ref _valueMin))
            {
                errors.Add(new ValidationResult("Неверно задан тип для параметра: " + DisplayName, new List<string> { "ValueMin" }));
            }
            if (!ValidateParam(ref _valueMax))
            {
                errors.Add(new ValidationResult("Неверно задан тип для параметра: " + DisplayName, new List<string> { "ValueMax" }));
            }

            if (errors.Count == 0)
            {
                if (((IComparable)ValueMax).CompareTo((IComparable)ValueMin) < 0)
                    errors.Add(new ValidationResult("Максимальный параметр не может быть меньше минимального!: " + DisplayName));
            }

            return errors;
        }

        
    }
}
