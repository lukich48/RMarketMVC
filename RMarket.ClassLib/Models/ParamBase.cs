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
    public abstract class ParamBase
    {
        public virtual string FieldName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Description { get; set; }
        public virtual string TypeName { get; set; }

        /// <summary>
        /// метод создан чтобы во время стандартной привязки нк получить тип string[]
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        protected virtual object GetValue(object fieldValue)
        {
            if (fieldValue != null && fieldValue.GetType() == typeof(string[]) && ((string[])fieldValue).Length > 0) //почему-то связывает с типом string[]
            {
                return ((string[])fieldValue)[0];
            }

            return fieldValue;
        }

        /// <summary>
        /// приводит значение параметра к его типу. Если приведение невозможно восстанавливвет умолчание.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="entity"></param>
        public abstract void RepairValue(MemberInfo prop, object entity);

        protected virtual bool ValidateParam(ref object fieldValue)
        {
            bool res = true;

            try
            {
                fieldValue = Convert.ChangeType(fieldValue, Type.GetType(TypeName), CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

    }
}
