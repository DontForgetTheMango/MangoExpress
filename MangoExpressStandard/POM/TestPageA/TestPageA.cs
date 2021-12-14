using System;
using MangoExpressStandard.Encapsulation;
using MangoExpressStandard.Extension;
using OpenQA.Selenium;

namespace MangoExpressStandard.POM
{
    /// <summary>
    /// Test page a
    /// </summary>
    public class TestPageA : BasePage, ITestPageA
    {
        public TestPageA(IWebDriver driver) : base(driver)
        {
        }

        private ISafeWebElement TestButton => Driver.ElementByXpath("//some/xpath");
        private ISafeWebElement TestField => Driver.ElementByXpath("//some/xpath", "exception message");

        /// <summary>
        /// Clicks the test button.
        /// </summary>
        public void ClickTestButton()
        {
            TestLogger.AddStep("Click 'Test Button'");
            TestButton.Click();
        }
    }
}
