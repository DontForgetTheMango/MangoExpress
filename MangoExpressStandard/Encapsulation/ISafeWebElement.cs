using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace MangoExpressStandard.Encapsulation
{
    public interface ISafeWebElement : IWebElement, ILocatable, IWrapsElement
    {
        int MaxRetries { get; set; }

        string Xpath { get; }
    }
}
