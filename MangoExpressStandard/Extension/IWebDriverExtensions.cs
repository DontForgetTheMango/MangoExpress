using System;
using System.Configuration;
using MangoExpressStandard.Encapsulation;
using MangoExpressStandard.Util;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace MangoExpressStandard.Extension
{
    public static class IWebDriverExtensions
    {
        public static object ExpectedConditions { get; private set; }

        public static ISafeWebElement ElementByXpath(
            this IWebDriver driver,
            string xpath,
            TimeSpan? timespan = null)
        {
            return CommonWaitForElementLogic(driver, xpath, GetTimeout(timespan));
        }

        /// <summary>
        /// Elements the by xpath.
        /// </summary>
        /// <returns>The by xpath.</returns>
        /// <param name="driver">Driver.</param>
        /// <param name="xpath">Xpath.</param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="timespan">Timespan.</param>
        public static ISafeWebElement ElementByXpath(
            this IWebDriver driver,
            string xpath,
            string exceptionMessage,
            TimeSpan? timespan = null)
        {
            var timeout = GetTimeout(timespan);
            var el = CommonWaitForElementLogic(driver, xpath, GetTimeout(timeout));

            var retEl = el;

            if (el == null)
            {
                throw new FindElementException(exceptionMessage, new Exception(
                    $"Cannot find xpath '{xpath}' within {timeout.TotalMilliseconds} milliseconds!"));
            }

            return retEl;
        }

        private static TimeSpan GetTimeout(TimeSpan? timespan)
        {
            if (timespan == null)
            {
                if (double.TryParse(ConfigurationManager.AppSettings["timeout"], out double result))
                {
                    return TimeSpan.FromMilliseconds(result);
                }

                throw new ConfigurationErrorsException("'timeout' is required in app.config!");
            }

            return TimeSpan.FromMilliseconds(timespan.Value.TotalMilliseconds);
        }

        private static ISafeWebElement CommonWaitForElementLogic(
            IWebDriver driver, 
            string xpath, 
            TimeSpan timespan)
        {
            var logger = Logger.MangoLogger.GetLogger();

            using (var timer = new DisposableStopWatch(NLog.LogLevel.Debug, $"WaitForElement:{xpath}"))
            {
                bool found = false;
                ISafeWebElement returnEl = null;
                bool isEnabled = false;
                bool isStale = false;
                bool isDisplayed = false;

                int i = 0;

                do
                {
                    i++;

                    try
                    {
                        found = ElementDetection.DoesXpathExist(driver, xpath, out returnEl);

                        isDisplayed = returnEl?.Displayed ?? false;

                        var stalenessFunc = SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(returnEl);

                        isStale = stalenessFunc(driver);

                        isEnabled = returnEl?.Enabled ?? true;
                    }
                    catch
                    {
                        found = false;
                        continue;
                    }

                    if (isStale)
                    {
                        found = false;
                    }

                    if (isDisplayed && isStale && !isEnabled)
                    {
                        found = true;
                    }

                    if (!isDisplayed && !isStale && !isEnabled)
                    {
                        found = false;
                    }
                } while (timer.Elapsed.TotalMilliseconds < timespan.TotalMilliseconds && !found);

                return returnEl;
            }
        }

        public static IAlert WaitForAlert(this IWebDriver driver, double waitTimeInSeconds = 5)
        {
            IAlert alert;

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTimeInSeconds));

            try
            {
                alert = wait.Until(d =>
                {
                    try
                    {
                        var a = driver.SwitchTo().Alert();
                        return a;
                    }
                    catch (NoAlertPresentException)
                    {
                        return null;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                alert = null;
            }

            return alert;
        }

        /// <summary>
        /// Moves to element.
        /// </summary>
        /// <returns>The to element.</returns>
        /// <param name="driver">Driver.</param>
        /// <param name="element">Element.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T MoveToElement<T>(this IWebDriver driver, T element) where T : IWebElement
        {
            var el = element as IWebElement;
            if (el is IWrapsElement a)
            {
                el = a.WrappedElement;
            }

            Actions actions = new Actions(driver);
            actions.MoveToElement(el);
            actions.Build().Perform();

            System.Threading.Thread.Sleep(250);

            return element;
        }

        /// <summary>
        /// Scrolls to top.
        /// </summary>
        /// <param name="driver">Driver.</param>
        public static void ScrollToTop(this IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0)");
        }
    }
}
