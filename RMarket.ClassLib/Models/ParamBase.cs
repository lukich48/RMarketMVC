using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Helpers;
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
        [Obsolete]
        public virtual string TypeName { get; set; }

        /// <summary>
        /// приводит значение параметра к его типу. Если приведение невозможно восстанавливает умолчание.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="entity"></param>
        public abstract void RepairValue(PropertyInfo prop, object entity);

        /// <summary>
        /// метод создан чтобы во время стандартной привязки не получить тип string[]
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        protected object GetValue(object fieldValue)
        {
            if (fieldValue != null && fieldValue.GetType() == typeof(string[]) && ((string[])fieldValue).Length > 0) //почему-то связывает с типом string[]
            {
                return ((string[])fieldValue)[0];
            }

            return fieldValue;
        }

        /// <summary>
        /// Приводит значение к своему типу. Генерируе исключение, если приведение невозможно
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        protected object CastToType(object fieldValue, Type typeValue)
        {
            object value = null;

            if(fieldValue.GetType() == typeValue)
            {
                return fieldValue;
            }

            try
            {
                if (fieldValue.GetType() == typeof(string))
                {
                    //нужна десеализация
                    return Serializer.Deserialize(fieldValue.ToString(), typeValue);
                }
            }
            catch (Exception)
            {
                //!!!Логировать
            }

            if (typeValue == typeof(TimeSpan))
            {
                value = TimeSpan.Parse(fieldValue.ToString());
            }
            else 
                value = Convert.ChangeType(fieldValue, typeValue, CultureInfo.InvariantCulture);

            return value;
        }

    }
}
