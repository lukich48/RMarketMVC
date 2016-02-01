using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers.Extentions
{
    public static class ExtensionMetods
    {
        public static double MillisecondUTC(this DateTime curDate)
        {
            DateTime date1970 = new DateTime(1970, 1, 1);
            TimeSpan ts = curDate - date1970;
            return ts.TotalMilliseconds;
        }

        public static bool IsIntegral(this object obj)
        {
            bool res = false;
            Type type = obj.GetType();

            if (type == typeof(sbyte)
                || type == typeof(byte)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(long)
                || type == typeof(ulong)
                )
            {
                res = true;
            }

            return res;
        }

        public static int ToIntSave(this object obj)
        {
            int res = 0;
            bool finish = false;
            try
            {
                res = (int)obj;
                finish = true;
            }
            catch (Exception) { }

            if (!finish)
            {
                try
                {
                    res = Convert.ToInt32(obj);
                }
                catch (Exception) { }
            }

            return res;

        }

        #region Копирование объектов

        /// <summary>
        /// Копирует значение полей и свойств
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="newObj"></param>
        /// <param name="oldObj"></param>
        /// <param name="excludeFields">объект с полями, которые нужно исключить из копирования</param>
        /// <param name="includeFields">объект с полями, которые нужно скопировать</param>
        public static void FillFields<T1, T2>(this T1 newObj, T2 oldObj, Func<T1, object> excludeFields = null, Func<T1, object> includeFields = null)
        { 
            PropertyInfo[] propsObj = oldObj.GetType().GetProperties();
            PropertyInfo[] propsNew = newObj.GetType().GetProperties();

            PropertyInfo[] propsExclude = null;
            if (excludeFields != null)
            {
                object excludeObject = excludeFields(newObj);
                propsExclude = excludeObject.GetType().GetProperties();
            }

            PropertyInfo[] propsInclude = null;
            if (includeFields != null)
            {
                object includeObject = includeFields(newObj);
                propsInclude = includeFields.GetType().GetProperties();
            }
            foreach (PropertyInfo prop in propsObj)
            {
                if(propsInclude!=null)
                {
                    if (!propsInclude.Any(p => p.Name == prop.Name))
                        continue;
                }

                if (propsExclude != null)
                {
                    if (propsExclude.Any(p => p.Name == prop.Name))
                        continue;
                }

                PropertyInfo newProp = Array.Find(propsNew, p => p.Name == prop.Name && p.PropertyType == prop.PropertyType);
                if(newProp!=null)
                {
                    newProp.SetValue(newObj, prop.GetValue(oldObj));
                }
            }

            FieldInfo[] fieldsObj = oldObj.GetType().GetFields();
            FieldInfo[] fieldsNew = newObj.GetType().GetFields();

            foreach (FieldInfo field in fieldsObj)
            {
                FieldInfo newField = Array.Find(fieldsNew, p => p.Name == field.Name && p.FieldType == field.FieldType);
                if (newField != null)
                {
                    newField.SetValue(newObj, field.GetValue(oldObj));
                }
            }
        }

        #endregion
    }
}
