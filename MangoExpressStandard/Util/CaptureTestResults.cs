using MangoExpressStandard.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace MangoExpressStandard.Util
{
    /// <summary>
    /// Capture Test Results executes and allows for pass/warn/fail of nunit tests
    /// </summary>
    public class CaptureTestResults
    {
        private readonly IWebDriver _driver;
        private readonly ITestDetails _testDetails;
        private readonly Action _action;
        private readonly string _dateString;
        private readonly string _caller;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driver">WebDriver instance</param>
        /// <param name="testDetails">ITestDetails</param>
        /// <param name="action">Action to be invoked</param>
        /// <param name="caller">Caller method</param>
        public CaptureTestResults(
            IWebDriver driver, 
            ITestDetails testDetails, 
            Action action, 
            [CallerMemberName] string caller = "")
        {
            _driver = driver;
            _testDetails = testDetails;
            _action = action;
            _dateString = DateTime.Now.ToString("s").Replace(":", "-");
            _caller = caller;
        }

        /// <summary>
        /// invoke the test
        /// </summary>
        [DebuggerStepThrough]
        public void Invoke()
        {
            if (!AppSettings.Release)
            {
                _action();
            }
            else
            {
                try
                {
                    SafelyCatchNunitExceptions(() => 
                    {
                        _action();
                    },
                    _testDetails,
                    _testDetails.Name);

                    CaptureTestData(string.Empty, NLog.LogLevel.Info);
                }
                catch (Exception ex)
                {
                    switch (_testDetails.UponFailure)
                    {
                        case DTO.TestDetails.TestFailureEnum.Fail:
                            CaptureTestData(ex.Message, NLog.LogLevel.Error);
                            throw;
                        case DTO.TestDetails.TestFailureEnum.Warn:
                            CaptureTestData(ex.Message, NLog.LogLevel.Warn);
                            Assert.Warn(ex.ToString());
                            break;
                        case DTO.TestDetails.TestFailureEnum.Ignore:
                            CaptureTestData(ex.Message, NLog.LogLevel.Debug);
                            Assert.Ignore(ex.ToString());
                            break;
                        default:
                            CaptureTestData(ex.Message, NLog.LogLevel.Error);
                            break;
                    }
                }
            }
        }

        private void CaptureTestData(string exceptionMessage, NLog.LogLevel loglevel)
        {
            string subfolderName;
            if (loglevel == NLog.LogLevel.Error)
                subfolderName = "FAILURE";
            else if (loglevel == NLog.LogLevel.Warn)
                subfolderName = "WARNING";
            else if (loglevel == NLog.LogLevel.Debug)
                subfolderName = "IGNORE";
            else
            {
                // anything other than Error, Warn, or Debug
                subfolderName = "PASS";
            }

            var thisSubfolderName = $@"{AppSettings.TestResultDirectory}\{subfolderName}";
            Directory.CreateDirectory(thisSubfolderName);

            Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();
            var imageLocation = $@"{thisSubfolderName}\final_image.png";
            ss.SaveAsFile(imageLocation, ScreenshotImageFormat.Png);

            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                var jsonLocation = $@"{thisSubfolderName}\exception.txt";
                File.WriteAllText(jsonLocation, exceptionMessage);
            }

            if (_testDetails != null)
            {
                var scenarioLocation = $@"{thisSubfolderName}\scenario.json";
                File.WriteAllText(scenarioLocation, JsonConvert.SerializeObject(_testDetails));
            }
        }

        private void SafelyCatchNunitExceptions(
            TestDelegate testDelegate,
            ITestDetails testDetails,
            string testName)
        {
            Exception caughtException;

            using (new TestExecutionContext.IsolatedContext())
            {
            }
        }
    }
}
