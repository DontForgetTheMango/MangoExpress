
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;

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

        public int MaxRetries { get; set; } = 3;

        #region ILocatable
        public Point LocationOnScreenOnceScrolledIntoView
        {
            get
            {
                var el = WrappedElement as ILocatable;
                return el.LocationOnScreenOnceScrolledIntoView;
            }
        }

        public ICoordinates Coordinates
        {
            get
            {
                var el = WrappedElement as ILocatable;
                return el.Coordinates;
            }
        }
        #endregion
        #region IWrapsElement
        public IWebElement WrappedElements { private set; get; }
        #endregion
        #region IWebElement
        public bool Displayed => Safe(() => WrappedElement.Displayed);

        public bool Enabled => Safe(() => WrappedElement.Enabled);

        public Point Location => Safe(() => WrappedElement.Location);

        public bool Selected => Safe(() => WrappedElement.Selected);

        public Size Size => Safe(() => WrappedElement.Size);

        public string TagName => Safe(() => WrappedElement.TagName);

        public string Text => Safe(() => WrappedElement.Text);

        public void Clear()
        {
            Safe(() => WrappedElement.Clear());
        }

        public void Click()
        {
            Safe(() => WrappedElement.Click());
        }

        public IWebElement FindElement(By @by)
        {
            return Safe(() => WrappedElement.FindElement(@by));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return Safe(() => WrappedElement.FindElements(@by));
        }

        public string GetAttribute(string name)
        {
            return Safe(() => WrappedElement.GetAttribute(name));
        }

        public string GetCssValue(string name)
        {
            return Safe(() => WrappedElement.GetCssValue(name));
        }

        public string GetProperty(string name)
        {
            return Safe(() => WrappedElement.GetProperty(name));
        }

        public void SendKeys(string text)
        {
            Safe(() => WrappedElement.SendKeys(text));
        }

        public void Submit()
        {
            Safe(() => WrappedElement.Submit());
        }
        #endregion
        private void RefreshElementReference()
        {
            var el = _driver.FindElement(By.XPath(Xpath));
            WrappedElement = el;
        }

        private void Safe(Action action)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var retryCount = 0;

            while (true)
            {
                try
                {
                    action();
                    return;
                }
                catch (InvalidElementStateException)
                {
                    if (++retryCount >= MaxRetries - 1)
                        throw;

                    logger.Error($"Refresh invalid element:{Xpath}");
                    RefreshElementReference();
                }
                catch(StaleElementReferenceException)
                {
                    if (++retryCount >= MaxRetries - 1)
                        throw;

                    logger.Error($"Refresh invalid element:{Xpath}");
                    RefreshElementReference();
                }

                Thread.Yield();
            }
        }

        private T Safe<T>(Func<T> action)
        {
            var result = default(T);

            Safe(() => 
            {
                result = action();
            })

            return result;
        }
    }
}
