using Newtonsoft.Json;
using RMarket.ClassLib.Abstract;
using RMarket.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RMarket.WebUI.Models
{
    [Bind(Exclude = "OriginValueMin,OriginValueMax")]
    public class ParamSelectionUI: IValidatableObject
    {
        //private IEntityParamConverter<object> converter; 
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string ValueMin { get; set; }
        [Required]
        public string ValueMax { get; set; }
        public string Description { get; set; } 
        [Required]
        public string TypeName { get; set; }
        public object OriginValueMin { get; set; }
        public object OriginValueMax { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            //найдем конвертер
            try
            {
                OriginValueMin = new ParamEntityConverterHelper().ConvertToDomainModel(ValueMin, TypeName);
            }
            catch (Exception)
            {
                errors.Add(new ValidationResult("Неверно задан тип для параметра: " + DisplayName, new List<string> { "ValueMin" }));
            }

            try
            {
                OriginValueMax = new ParamEntityConverterHelper().ConvertToDomainModel(ValueMax, TypeName);
            }
            catch (Exception)
            {
                errors.Add(new ValidationResult("Неверно задан тип для параметра: " + DisplayName, new List<string> { "ValueMax" }));
            }

            if (errors.Count == 0)
            {
                if (((IComparable)OriginValueMax).CompareTo((IComparable)OriginValueMin) < 0)
                    errors.Add(new ValidationResult("Максимальный параметр не может быть меньше минимального!: " + DisplayName));
            }

            return errors;
        }


    }
}
