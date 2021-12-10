
using System;
using OpenQA.Selenium;

namespace MangoExpressStandard.Util
{
    public class SafeWebElement : ISafeWebElement
    {
        private IWebDriver _driver;

        public string Xpath { get; private set; }

        public IWebElement WrappedElement { get; set; }

        public SafeWebElement(IWebElement wrappedElement, IWebDriver driver, string xpath)
        {
            WrappedElement = wrappedElement;
            _driver = driver;
            Xpath = xpath;
        }

        public int MaxRetries { get; set; }


    }
}
