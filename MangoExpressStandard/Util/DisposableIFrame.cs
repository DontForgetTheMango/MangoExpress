using System;
using MangoExpressStandard.Encapsulation;
using OpenQA.Selenium;

namespace MangoExpressStandard.Util
{
    public class DisposableIFrame : IDisposable
    {
        private IWebDriver _driver;

        public DisposableIFrame(IWebDriver driver, string xpath)
        {
            _driver = driver;
            if (ElementDetection.DoesXpathExist(driver, xpath, out ISafeWebElement frame))
                driver.SwitchTo().Frame(frame);
        }

        public DisposableIFrame(IWebDriver driver, IWebElement iframeEl)
        {
            _driver = driver;
            driver.SwitchTo().Frame(iframeEl);
        }

        public void Dispose()
        {
            _driver.SwitchTo().DefaultContent();
        }
    }
}
