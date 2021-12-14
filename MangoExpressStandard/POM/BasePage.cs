using System;
using OpenQA.Selenium;

namespace MangoExpressStandard.POM
{
    /// <summary>
    // class for all common page interactions
    /// </summary>
    public class BasePage : BaseComponent, IBasePage
    {
        public BasePage(IWebDriver driver) : base(driver)
        {
        }
    }
}
