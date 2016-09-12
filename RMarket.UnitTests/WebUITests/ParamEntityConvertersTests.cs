using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMarket.WebUI.Infrastructure.ParamEntityConverters;
using System.Collections.Generic;
using RMarket.WebUI.Helpers;

namespace RMarket.UnitTests.WebUITests
{
    [TestClass]
    public class ParamEntityConvertersTests
    {
        [TestMethod]
        public void AdapterToObjectConverterTest()
        {
            decimal value = 1000.01m;

            var converter = new ParamEntityConverterHelper();

            string strValue = converter.ConvertToViewModel(value);
            object result = converter.ConvertToDomainModel(strValue, value.GetType().AssemblyQualifiedName);
        }

        [TestMethod]
        public void TimeSpanConverterTest()
        {
            TimeSpanConverter converter = new TimeSpanConverter();
            TimeSpan value = new TimeSpan(10000);

            string strValue = converter.ToViewModel(value);
            TimeSpan result = converter.ToDomainModel(strValue);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void DictinaryConverterTest()
        {
            DictionaryConverter converter = new DictionaryConverter();
            Dictionary<string, string> value = new Dictionary<string, string>();
            value.Add("Sber", "1");
            value.Add("Avaz", "2");

            string strValue = converter.ToViewModel(value);
            IDictionary<string, string> result = converter.ToDomainModel(strValue);

            Assert.AreEqual(value.Count, result.Count);
            Assert.AreEqual(value["Sber"], result["Sber"]);
        }

    }
}
