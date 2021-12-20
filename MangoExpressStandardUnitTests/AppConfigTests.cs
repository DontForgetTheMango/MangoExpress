using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace MangoExpressStandardUnitTests
{
    public class AppConfigTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConfigExists()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"path:{dir}");
            Assert.True(File.Exists($@"{dir}/MangoExpressStandard.dll.config"));
        }

        [Test]
        public void TestConfigContainsAppSettings()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"path:{dir}");
            var text = File.ReadAllText($@"{dir}/MangoExpressStandard.dll.config");
            StringAssert.Contains("<appSettings>", text);
        }
    }
}
