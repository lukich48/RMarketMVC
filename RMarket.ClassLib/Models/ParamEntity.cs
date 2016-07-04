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
    public class ParamEntity :ParamBase, IValidatableObject
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

            try
            {
                FieldValue = Convert.ChangeType(FieldValue, prop.PropertyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                FieldValue = null;
            }

            if (FieldValue == null)
                FieldValue = prop.GetValue(entity);

            DisplayName = (attr.Name == null) ? prop.Name : attr.Name;
            Description = (attr.Description == null) ? "": attr.Description;
            TypeName = prop.PropertyType.FullName;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (!ValidateParam(ref _fieldValue))
            {
                errors.Add(new ValidationResult("Неверно задан тип для параметра: " + DisplayName, new List<string> { "FieldValue" }));
            }

            return errors;
        }

    }

}
