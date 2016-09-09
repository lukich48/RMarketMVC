using RMarket.WebUI.Abstract;
using RMarket.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Models
{
    [Bind(Exclude = "OriginValue")]
    public class ParamEntityModel: IValidatableObject
    {
        //private IEntityParamConverter<object> converter; 
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string FieldValue { get; set; }
        public string Description { get; set; }

        [Required]
        public string TypeName { get; set; }
        public object OriginValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            //найдем конвертер
            try
            {
                OriginValue = new ParamEntityConverterHelper().ConvertToDomainModel(FieldValue, TypeName);
            }
            catch (Exception)
            {
                errors.Add(new ValidationResult("Неверно задан тип для параметра: " + DisplayName, new List<string> { "FieldValue" }));
            }

            return errors;
        }

    }
}