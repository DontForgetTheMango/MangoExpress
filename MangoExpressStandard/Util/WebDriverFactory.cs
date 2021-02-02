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

            var screensize = ConfigurationManager.AppSettings["screensize"];
            if (!string.IsNullOrEmpty(screensize))
            {
                var match = reg.Match(screensize);
                if (match.Success)
                {
                    var xAndY = match.Value.ToLower().Split('x');
                    int x = int.Parse(xAndY[0]);
                    int y = int.Parse(xAndY[1]);
                    _driver = OpenDesiredWebDriver();
                    _driver.Manage().Window.Size = new System.Drawing.Size(x, y);
                }
                else
                    throw new ConfigurationErrorsException("screensize should be formatted intXint");
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

        public enum BrowserOptions
        {
            Chrome, 
            IE,
            FireFox
        }

        private static IWebDriver OpenDesiredWebDriver()
        {
            var downloadDirectory = AppSettings.DownloadsRootDirectory;
            switch (AppSettings.Browser)
            {
                case BrowserOptions.Chrome:
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
                case BrowserOptions.IE:
                    throw new NotImplementedException("No one is using IE! Stop it!");
                case BrowserOptions.FireFox:
                    {
                        throw new NotImplementedException("FireFox has not be implemented yet. Sorry...");
                    }
                default:
                    throw new ConfigurationErrorsException("No browser selected in app.config");
            }
        }
    }
}
