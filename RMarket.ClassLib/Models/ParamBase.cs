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

        /// <summary>
        /// приводит значение параметра к его типу. Не восстанавливвет значение.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="entityType"></param>
        public abstract void RepairValue(PropertyInfo prop, Type entityType);

        /// <summary>
        /// приводит значение параметра к его типу. Если приведение невозможно восстанавливает умолчание.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="entity"></param>
        public abstract void RepairValue(PropertyInfo prop, object entity);

        /// <summary>
        /// заполняет значение дефолтовым значением, если оно null
        /// </summary>
        /// <param name="entity"></param>
        public abstract void RepairValue(object entity);
    }
}
