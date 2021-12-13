using System;
using OpenQA.Selenium;

namespace MangoExpressStandard.POM
{
    /// <summary>
    /// Base component interface
    /// </summary>
    public interface IBaseComponent
    {
        /// <summary>
        /// Gets the driver.
        /// </summary>
        /// <value>The driver.</value>
        IWebDriver Driver { get; }

        /// <summary>
        /// Initialize the specified driver.
        /// </summary>
        /// <param name="driver">Driver.</param>
        void Initialize(IWebDriver driver);
    }
}
