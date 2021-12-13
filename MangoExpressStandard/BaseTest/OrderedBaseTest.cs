using System;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandard.BaseTest
{
    public class OrderedBaseTest : PrivateBaseTest
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            Driver = WebDriverFactory.NewWebDriver();
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            Driver.Quit();
            Driver.Dispose();

            var testFixtureName = TestContext.CurrentContext.Test.Name;
            var status = TestContext.CurrentContext.Result.Outcome.Status.ToString("g");
            PrerequisiteTestFixtures.SetStatus(testFixtureName, status);
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            if (!ContinueOrderedTests && AppSettings.Release) 
                Assert.Ignore();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
