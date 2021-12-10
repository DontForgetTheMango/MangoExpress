using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MangoExpressStandard.Encapsulation;
using NLog;
using OpenQA.Selenium;

namespace MangoExpressStandard.Util
{
    public class ElementDetection
    {
        public static bool IsElementVisible(IWebElement element)
        {
            return element.Displayed && element.Enabled;
        }

        public static bool DoesXpathExist(
            IWebDriver driver,
            string xpath,
            out ISafeWebElement outElement
        )
        {
            var logger = LogManager.GetCurrentClassLogger();

            var htmlDoc = new HtmlDocument();
            string innerHtml = driver.PageSource;
            htmlDoc.LoadHtml(innerHtml);
            try
            {
                var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);
                if (node != null)
                {
                    try
                    {
                        var el = driver.FindElement(By.XPath(node.XPath));
                        outElement = new SafeWebElement(el, driver, xpath);
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        logger.Debug("NoSuchElementException");
                        outElement = null;
                        return false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        logger.Debug("NoSuchElementException");
                        outElement = null;
                        return false;
                    }
                }
                else
                {
                    outElement = null;
                    return false;
                }
            }
            catch (System.Xml.XPath.XPathException ex)
            {
                throw new FindElementException(ex.Message, new Exception(xpath));
            }
        }

        #region TryFindElement
        private static bool DoesXpathExistFromElement(IWebElement element, string xpath)
        {
            var htmlDoc = new HtmlDocument();
            string innerHtml = element.GetAttribute("innerHtml");
            htmlDoc.LoadHtml(innerHtml);
            return htmlDoc.DocumentNode.SelectNodes(xpath) != null;
        }

        public static bool TryFindElement(IWebElement el, string xpath, out List<IWebElement> element)
        {
            if (DoesXpathExistFromElement(el, xpath))
            {
                element = el.FindElements(By.XPath(xpath)).ToList();
                return true;
            }
            else
            {
                element = null;
                return false;
            }
        }

        public static bool TryFindElements(IWebDriver driver, string xpath, out List<IWebElement> element)
        {
            element = new List<IWebElement>();

            var htmlDoc = new HtmlDocument();
            string innerHtml = driver.PageSource;
            htmlDoc.LoadHtml(innerHtml);
            var els = htmlDoc.DocumentNode.SelectNodes(xpath);

            if (els != null)
            {
                foreach(var el in els)
                {
                    element.Add(driver.FindElement(By.XPath(el.XPath)));
                }
                return true;
            }
            else
            {
                element = null;
                return false;
            }
        }

        public static bool TryFindElements(IWebDriver driver, string xpath, string exceptionMessage, out List<IWebElement> element)
        {
            element = new List<IWebElement>();

            var htmlDoc = new HtmlDocument();
            string innerHtml = driver.PageSource;
            htmlDoc.LoadHtml(innerHtml);
            var els = htmlDoc.DocumentNode.SelectNodes(xpath);

            if (els != null)
            {
                foreach(var el in els)
                {
                    element.Add(driver.FindElement(By.XPath(el.XPath)));
                }
                return true;
            }
            else
            {
                throw new FindElementException(exceptionMessage);
            }
        }
        #endregion
    }
}
