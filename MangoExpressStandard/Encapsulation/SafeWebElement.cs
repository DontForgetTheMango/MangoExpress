using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Threading;

namespace MangoExpressStandard.Encapsulation
{
    /// <summary>
    /// SafeWebElement is a Selenium WebElement that auto-retry upon failure
    /// </summary>
    public class SafeWebElement : ISafeWebElement
    {
        private IWebDriver _driver;

        public string Xpath { get; private set; }

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
        public IWebElement WrappedElement { get; private set; }
        #endregion
        #region IWebElement
        public bool Displayed => Safe(() => WrappedElement.Displayed);

        public bool Enabled => Safe(() => WrappedElement.Enabled);

        public Point Location => Safe(() => WrappedElement.Location);

        public bool Selected => Safe(() => WrappedElement.Selected);

        public Size Size => Safe(() => WrappedElement.Size);

        public string TagName => Safe(() => WrappedElement.TagName);

        public string Text => Safe(() => WrappedElement.Text);

        public void Click()
        {
            Safe(() => WrappedElement.Click());
        }
        public void Clear()
        {
            Safe(() => WrappedElement.Clear());
        }

        public IWebElement FindElement(By @by)
        {
            return Safe(() => WrappedElement.FindElement(@by));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            return Safe(() => WrappedElement.FindElements(@by));
        }

        public string GetAttribute(string attributeName)
        {
            return Safe(() => WrappedElement.GetAttribute(attributeName));
        }

        public string GetCssValue(string propertyName)
        {
            return Safe(() => WrappedElement.GetCssValue(propertyName));
        }

        public string GetProperty(string propertyName)
        {
            return Safe(() => WrappedElement.GetProperty(propertyName));
        }

        public void SendKeys(string text)
        {
            Safe(() => WrappedElement.SendKeys(text));
        }

        public void Submit()
        {
            Safe(() => WrappedElement.Submit());
        }

        public string GetDomAttribute(string attr)
        {
            return Safe(() => WrappedElement.GetDomAttribute(attr));
        }

        public string GetDomProperty(string prop)
        {
            return Safe(() => WrappedElement.GetDomProperty(prop));
        }

        public ISearchContext GetShadowRoot()
        {
            return Safe(() =>  WrappedElement.GetShadowRoot());
        }
        #endregion

        private void RefreshElementReference()
        {
            var el = _driver.FindElement(By.XPath(Xpath));
            WrappedElement = el;
        }

        private void Safe(Action action)
        {
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

                    RefreshElementReference();
                }
                catch (StaleElementReferenceException)
                {
                    if (++retryCount >= MaxRetries - 1)
                        throw;

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
            });

            return result;
        }
    }
}
