using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;

namespace MangoExpressStandard.Util
{
    public class SeleniumEventHandlers
    {
        /// <summary>
        /// event before element is clicked
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public static void OnElementClicking(object sender, WebElementEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
                // do something with driver
            }
        }

        /// <summary>
        /// event after element is clicked
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public static void OnElementClicked(object sender, WebElementEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
                // do something with driver
            }
        }

        /// <summary>
        /// event before element value changed
        /// </summary>
        /// <param name="sender">IWebDriver</param>
        /// <param name="e">e.Element is IWebElement</param>
        public static void OnElementValueChanging(object sender, WebElementValueEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
                IWebElement el = e.Element;
                // do something with driver
            }
        }

        /// <summary>
        /// event after element value changed
        /// </summary>
        /// <param name="sender">IWebDriver</param>
        /// <param name="e">e.Element is IWebElement</param>
        public static void OnElementValueChanged(object sender, WebElementValueEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
                IWebElement element = e.Element;
            }
        }

        /// <summary>
        /// event before finding element
        /// </summary>
        /// <param name="sender">IWebDriver</param>
        /// <param name="e">e.Element is IWebElement</param>
        public static void OnFindingElement(object sender, FindElementEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
            }
        }

        /// <summary>
        /// event after element found
        /// </summary>
        /// <param name="sender">IWebDriver</param>
        /// <param name="e">e.Element is IWebElement</param>
        public static void OnFindElementCompleted(object sender, FindElementEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
            }
        }

        /// <summary>
        /// after exception thrown
        /// </summary>
        /// <param name="sender">IWebDriver</param>
        /// <param name="e">e.Script</param>
        public static void OnExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
                var message = e.ThrownException.Message;
            }
        }

        /// <summary>
        /// event before script is executed
        /// </summary>
        /// <param name="sender">IWebDriver</param>
        /// <param name="e">e.Script</param>
        public static void OnScriptExecuting(object sender, WebDriverScriptEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
                var scriptToBeExecuted = e.Script;
            }
        }

        /// <summary>
        /// event after script is executed
        /// </summary>
        /// <param name="sender">IWebDriver</param>
        /// <param name="e">e.Script</param>
        public static void OnScriptExecuted(object sender, WebDriverScriptEventArgs e)
        {
            if (sender is IWebDriver driver)
            {
                var scriptExecuted = e.Script;
            }
        }


    }
}
