using System;
using MangoExpressStandard.POM;
using MangoExpressStandard.Util;
using OpenQA.Selenium;

namespace MangoExpressStandard.Factory
{
    /// <summary>
    /// Get desired version of POM object set in test config
    /// </summary>
    public class PomFactory
    {
        public static ITestPageA GetTestPageA(IWebDriver driver)
        {
            switch(TestSettings.ExamplePageVersion)
            {
                case 1:
                    var iPageA = DependencyResolver.For<ITestPageA>(typeof(TestPageA));
                    iPageA.Initialize(driver);
                    return iPageA;
                default:
                    throw new NotImplementedException($"TestPageA version {TestSettings.ExamplePageVersion} not implemented!");
            }
        }
    }
}
