using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace MangoExpressStandard.Util
{
    public interface ISafeWebElement : ISafeWebElement, ILocatable, IWrapsElement
    {
        int MaxRetries { get; set; }

        string Xpath { get; }
    }
}
