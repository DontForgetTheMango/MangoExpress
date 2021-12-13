using System;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandard.BaseTest
{
    public class StandardBaseTest : PrivateBaseTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Driver = WebDriverFactory.NewWebDriver();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            Driver.Quit();
            Driver.Dispose();
        }
    }
}
