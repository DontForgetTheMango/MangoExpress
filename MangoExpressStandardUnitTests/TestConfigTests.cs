using System;
using System.IO;
using System.Reflection;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandardUnitTests
{
    public class TestConfigTests
    {
        [Test]
        public void ExampleTestVersionIs1()
        {
            var exampleTestVersion = TestSettings.ExampleTestVersion;

            Assert.That(exampleTestVersion, Is.EqualTo(1));
        }

        [Test]
        public void NonExistentTestVersionIsNegative1()
        {
            var exampleTestVersion = TestSettings.NonexistentTestVersion;

            Assert.That(exampleTestVersion, Is.EqualTo(-1));
        }
    }
}
