using System;
using RMarket.WebUI.Infrastructure.ParamEntityConverters;
using System.Collections.Generic;
using RMarket.WebUI.Helpers;
using NUnit.Framework;

namespace RMarket.UnitTests.WebUITests
{
    [TestFixture]
    public class ParamEntityConvertersTests
    {
        [Test]
        public void AdapterToObjectConverterTest()
        {
            decimal value = 1000.01m;

            var converter = new ParamEntityConverterHelper();

            string strValue = converter.ConvertToViewModel(value);
            object result = converter.ConvertToDomainModel(strValue, value.GetType().AssemblyQualifiedName);
        }

        [Test]
        public void TimeSpanConverterTest()
        {
            TimeSpanConverter converter = new TimeSpanConverter();
            TimeSpan value = new TimeSpan(10000);

            string strValue = converter.ToViewModel(value);
            TimeSpan result = converter.ToDomainModel(strValue, value.GetType());

            Assert.AreEqual(value, result);
        }

        [Test]
        public void DictinaryConverterTest()
        {
            DictionaryConverter converter = new DictionaryConverter();
            Dictionary<string, string> value = new Dictionary<string, string>();
            value.Add("Sber", "1");
            value.Add("Avaz", "2");

            string strValue = converter.ToViewModel(value);
            IDictionary<string, string> result = converter.ToDomainModel(strValue, value.GetType());

            Assert.AreEqual(value.Count, result.Count);
            Assert.AreEqual(value["Sber"], result["Sber"]);
        }

    }
}
