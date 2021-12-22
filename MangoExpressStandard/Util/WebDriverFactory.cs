using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace MangoExpressStandard.Util
{
    public class WebDriverFactory
    {
        public static IWebDriver NewWebDriver()
        {
            IWebDriver _driver;
            Regex reg = new Regex("[0-9]{1,4}[xX][0-9]{1,4}");

            var browsersize = AppSettings.BrowserSize;
            if (!browsersize.IsFullScreen)
            {
                _driver = OpenDesiredWebDriver();
                _driver.Manage().Window.Size = new System.Drawing.Size(browsersize.W, browsersize.H);
            }
            else
            {
                _driver = OpenDesiredWebDriver();
                _driver.Manage().Window.Maximize();
            }

            if (_driver == null)
                throw new ConfigurationErrorsException("WebDriver is null!");

            return _driver;
        }

        private static IWebDriver OpenDesiredWebDriver()
        {
            var downloadDirectory = AppSettings.DownloadsDirectory;
            switch (AppSettings.Browser)
            {
                case AppSettings.BrowserOptions.Chrome:
                {
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--disable-features=VizDisplayCompositor");
                    chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
                    chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                    chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                    chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;

                    if (AppSettings.HeadlessMode)
                        chromeOptions.AddArgument("headless");

                    return new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeOptions);
                }
                case AppSettings.BrowserOptions.IE:
                    throw new NotImplementedException("No one is using IE! Stop it!");
                case AppSettings.BrowserOptions.FireFox:
                    throw new NotImplementedException("FireFox has not be implemented yet. Sorry...");
                default:
                    throw new ConfigurationErrorsException("No browser selected in app.config");
            }
        }
    }
}
