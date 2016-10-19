using System;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Entities;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace RMarket.UnitTests.HelpersTests
{
    [TestFixture]
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
        
        [Test]
        public void GetNavigationPropertiesTest()
        {
            //1
            var props1= ReflectionHelper.GetNavigationProperties<TestProps1>();
            Assert.AreEqual(2, props1.Count());

            //2
            var props2 = ReflectionHelper.GetNavigationProperties<TestProps2>();

            Assert.AreEqual(1, props2.Count());

            //3
            var props3 = ReflectionHelper.GetNavigationProperties<TestProps3>();

            Assert.AreEqual(0, props3.Count());
        }
    }
}
