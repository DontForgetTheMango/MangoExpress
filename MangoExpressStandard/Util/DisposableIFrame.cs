using System;
using OpenQA.Selenium;

namespace MangoExpressStandard.Util
{
    public class DisposableIFrame : IDisposable
    {
        private IWebDriver _driver;

        public DisposableIFrame(IWebDriver driver, string xpath)
        {
            _driver = driver;
            if (ElementDetection.)
                driver.SwitchTo().Frame(frame);
        }
    }
}
