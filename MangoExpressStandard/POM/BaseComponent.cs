using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MangoExpressStandard.POM
{
    public class BaseComponent
    {
        private IWebDriver _driver;

        protected Logger.MangoLogger TestLogger { get; } = Logger.MangoLogger.GetLogger();

        protected WebDriverWait Wait { get; private set; }

        public BaseComponent(IWebDriver driver)
        {
            if (driver != null)
                Initialize(driver);
        }

        public void Initialize(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException("IWebDriver cannot be null!");
            var timeout = ConfigurationManager.AppSettings["timeout"] ?? "5000";
            Wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(double.Parse(timeout)));
        }

        public IWebDriver Driver
        {
            get
            {
                return _driver ?? throw new ArgumentNullException("Driver cannot be left null!");
            }
            protected set 
            {
                _driver = value;
            }
        }
    }
}
