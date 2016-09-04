using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public class Serializer
    {
        public static string Serialize(object serializeObject)
        {
            return JsonConvert.SerializeObject(serializeObject);
        }

        public static T Deserialize<T>(string jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(jsonObject);
        }

        public static object Deserialize(string jsonObject, Type type)
        {
            return JsonConvert.DeserializeObject(jsonObject, type);
        }


    }
}
