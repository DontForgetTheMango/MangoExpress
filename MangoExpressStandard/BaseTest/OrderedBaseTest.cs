using System;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandard.BaseTest
{
    /// <summary>
    /// OrderedBaseTest should be inherrited by TestFixture classes that
    /// contain ordered tests <code>[Order(<1, 2, n>)]</code>
    ///
    /// 1. Only one WebDriver will be instantiated per TestFixture and closed
    ///     once all tests in the fixture are executed
    /// 2. All downstream tests are dependent upon the prior test status.
    ///     I.E. if the second test fails, all remaining tests will be ignored. 
    /// </summary>
    public class OrderedBaseTest : PrivateBaseTest
    {
        /// <summary>
        /// Open WebDriver and attach event handlers
        /// </summary>
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            Driver = WebDriverFactory.NewWebDriver();

            AddEventHandlers(Driver);
        }

        /// <summary>
        /// Close WebDriver and document TestFixture status
        /// </summary>
        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            Driver.Quit();
            Driver.Dispose();

            var testFixtureName = TestContext.CurrentContext.Test.Name;
            var status = TestContext.CurrentContext.Result.Outcome.Status.ToString("g");
            PrerequisiteTestFixtures.SetStatus(testFixtureName, status);
        }

        /// <summary>
        /// If previous Test in TestFixture failed, this Test will be ignored.
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            if (!ContinueOrderedTests && AppSettings.Release) 
                Assert.Ignore("Prior test has failed!");
        }

        /// <summary>
        /// base.TearDown() documents Test status and
        /// sets ContinueOrderedTests false if executed test failed
        /// </summary>
        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
