using System;
using MangoExpressStandard.BaseTest;
using MangoExpressStandard.Factory;
using MangoExpressStandard.POM;
using NUnit.Framework;

namespace MangoExpressStandardUnitTests
{
    public class PomTests : StandardBaseTest
    {
        [Test]
        public void TestPageAThrowsWithNoDriver()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                var testPageA = new TestPageA();
                var _driver = testPageA.Driver;
            });
        }

        [Test]
        public void TestPageAFromNew()
        {
            Assert.DoesNotThrow(() =>
            {
                var testPageA = new TestPageA(Driver);
                var _driver = testPageA.Driver;
            });
        }

        [Test]
        public void TestPageAFromFactory()
        {
            Assert.DoesNotThrow(() =>
            {
                var testPageA = PomFactory.GetTestPageA(Driver);
                var _driver = testPageA.Driver;
            });
        }
    }
}
