using System;
using MangoExpressStandard.Encapsulation;
using MangoExpressStandard.Extension;
using MangoExpressStandard.Util;
using OpenQA.Selenium;

namespace MangoExpressStandard.POM
{
    /// <summary>
    /// Test page a
    /// </summary>
    public class TestPageA : BasePage, ITestPageA
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver">driver = null if using DependencyResolver</param>
        public TestPageA(IWebDriver driver = null) : base(driver)
        {
        }

        private ISafeWebElement TestButton => Driver.ElementByXpath("//some/xpath");
        private ISafeWebElement TestField => Driver.ElementByXpath("//some/xpath", "exception message");

        /// <summary>
        /// Clicks the test button.
        /// </summary>
        public void ClickTestButton()
        {
            SafetyHarness.DefaultHandleException(typeof(ElementNotVisibleException));
            SafetyHarness.Retry(() =>
            {
                TestLogger.AddStep("Click 'Test Button'");
                TestButton.Click();
            });
        }
    }
}
