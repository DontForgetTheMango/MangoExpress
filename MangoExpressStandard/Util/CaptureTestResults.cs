using MangoExpressStandard.DTO;
using MangoExpressStandard.Logger;
using Newtonsoft.Json;
using NLog;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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
        private readonly string _testName;
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
            _testName = GetTestNameFromAction(action);
            _caller = caller;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MangoExpressStandard.Util.CaptureTestResults"/> class.
        /// </summary>
        /// <param name="driver">Driver.</param>
        /// <param name="action">Action.</param>
        /// <param name="caller">Caller.</param>
        public CaptureTestResults(
        IWebDriver driver,
        Action action,
        [CallerMemberName] string caller = "")
        {
            _driver = driver;
            _testDetails = new TestDetails
            {
                Name = caller,
                UponFailure = TestDetails.TestFailureEnum.Fail
            };
            _action = action;
            _dateString = DateTime.Now.ToString("s").Replace(":", "-");
            _testName = GetTestNameFromAction(action);
            _caller = caller;
        }

        private string GetTestNameFromAction(Action action)
        {
            // get test name
            System.Reflection.MethodInfo className = action.Method;
            var testName = className.DeclaringType.FullName;

            // remove everything after "+" sign
            int i = testName.IndexOf("+", StringComparison.Ordinal);
            if (i > 0)
                testName = testName.Substring(0, i);
            testName = testName.Split('.').Last();

            // get method name
            string methodName = action.Method.Name;
            var pattern = @"\<(.*?)\>";
            var matches = Regex.Matches(methodName, pattern);

            // clean up method name
            foreach (Match m in matches)
            {
                methodName = m.Groups[1].ToString();
            }

            // append method name to test name
            testName += $".{methodName}";
            return testName;
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
                finally
                {
                    CaptureDataForHTMLResults();
                }
            }
        }

        [ThreadStatic]
        public static string TestResultDirectory;

        private void SetTestResultDirectory()
        {
            if (string.IsNullOrEmpty(TestResultDirectory))
                TestResultDirectory = $@"{AppSettings.TestResultsDirectory}\{_testName}_{_dateString}";
        }

        private void CaptureDataForHTMLResults()
        {
            // create HTML directory
            var imageDirectory = $@"{AppSettings.TestResultsDirectory}\HTML\images";
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();
            ss.SaveAsFile($@"{imageDirectory}\{TestContext.CurrentContext.Test.ID}.png", ScreenshotImageFormat.Png);
        }

        private void CaptureAllData(string ex, string subfolderName, NLog.LogLevel logLevel)
        {
            var logger = LogManager.GetCurrentClassLogger();

            // create subfolder
            var thisSubfolderName = $@"{TestResultDirectory}\{subfolderName}";
            Directory.CreateDirectory(thisSubfolderName);

            Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();
            string imageLocation = $@"{thisSubfolderName}\final_image.png";
            logger.Log(logLevel, $"image: {imageLocation}");
            ss.SaveAsFile(imageLocation, ScreenshotImageFormat.Png);

            // save exception 
            if (!string.IsNullOrEmpty(ex))
            {
                string jsonLocation = $@"{thisSubfolderName}\exception.txt";
                logger.Log(logLevel, $@"json: {jsonLocation}");
                File.WriteAllText(jsonLocation, ex);
            }

            // save scenario
            if (_testDetails != null)
            {
                string scenarioLocation = $@"{thisSubfolderName}\testDetails.json";
                logger.Log(logLevel, $"json: {scenarioLocation}");
                File.WriteAllText(scenarioLocation, JsonConvert.SerializeObject(_testDetails));
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

            var thisSubfolderName = $@"{AppSettings.TestResultsDirectory}\{subfolderName}";
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
            TestDelegate code,
            ITestDetails testDetails,
            string testName)
        {
            var logger = MangoLogger.GetLogger();

            using (new TestExecutionContext.IsolatedContext())
            {
                var throwException = false;
                try
                {
                    code();
                }
                catch (Exception ex)
                {
                    logger.Debug($"ex.Message: {ex.Message}");
                    var caughtException = ex;

                    var messages = new List<string>();
                    if (caughtException.Message.StartsWith("Multiple failures or warnings in test:"))
                    {
                        var last = false;
                        for (int i = 1; i < 100; i++)
                        {
                            var message = caughtException.Message.Substring(caughtException.Message.IndexOf($"{i}") + 2);

                            if (message.Contains($"{i + 1})"))
                                message = message.Substring(0, message.IndexOf($"{i + 1}"));
                            else
                                last = true;
                        }
                    }
                    else
                    {
                        messages.AddRange(GetAllExceptionMessages(caughtException));
                    }

                    foreach (var message in messages)
                    {
                        logger.Debug($"caughtException.Message: {message}");

                        var knownIssueList = new List<ExpectedError>();
                        foreach (var knownIssue in knownIssueList)
                        {
                            if (string.IsNullOrEmpty(knownIssue.Message))
                                continue;

                            Regex rgx = new Regex(knownIssue.Message);
                            var isKnownIssue = rgx.IsMatch(message);
                            if (isKnownIssue)
                                knownIssueList.Add(knownIssue);
                        }

                        logger.Debug($"knownIssueList: {string.Join(",", knownIssueList)}");

                        if (knownIssueList.Count > 0)
                        {
                            foreach (var knownIssue in knownIssueList)
                            {
                                if (!knownIssue.IsResolved)
                                {
                                    // Write exception to file for archiving
                                }
                                else
                                {
                                    throwException = true;
                                }
                            }
                        }
                        else
                        {
                            throwException = true;
                            break;
                        }
                    }

                    // throw the exception
                    if (throwException)
                        throw;
                }
            }
        }

        private List<string> GetAllExceptionMessages(Exception ex)
        {
            var retMessage = new List<string>
            {
                ex.Message
            };

            var exception = ex;
            while (exception.InnerException != null)
            {
                retMessage.Add(exception.InnerException.Message);
                exception = exception.InnerException;
            }

            return retMessage;
        }
    }
}
