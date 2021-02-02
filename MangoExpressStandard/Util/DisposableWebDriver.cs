using OpenQA.Selenium;
using System;

namespace MangoExpressStandard.Util
{
    public class DisposableWebDriver : IDisposable
    {
        public IWebDriver Driver { get; protected set; }

        public DisposableWebDriver()
        {
            Driver = WebDriverFactory.NewWebDriver();
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
