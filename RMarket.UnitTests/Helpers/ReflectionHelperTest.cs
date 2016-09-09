using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Entities;
using System.Linq;

namespace RMarket.UnitTests.Helpers
{
    [TestClass]
    public class ReflectionHelperTest
    {
        class TestProps1
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
            public Instance PropNavigation1 { get; set; }
            public Selection PropNavigation2 { get; set; }
        }

        class TestProps2
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
            public Instance PropNavigation1 { get; set; }
        }

        class TestProps3
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
        }


        [TestMethod]
        public void GetNavigationPropertiesTest()
        {
            //1
            var props= ReflectionHelper.GetNavigationProperties<TestProps1>();

            Assert.AreEqual(2, props.Count());

            //2
            var props2 = ReflectionHelper.GetNavigationProperties< TestProps2>();

            Assert.AreEqual(1, props2.Count());

            //3
            var props3 = ReflectionHelper.GetNavigationProperties<TestProps3>();

            Assert.AreEqual(0, props3.Count());
        }
    }
}
