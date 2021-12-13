using System;
using System.IO;
using System.Linq;
using MangoExpressStandard.Attribute;
using MangoExpressStandard.Util;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;

namespace MangoExpressStandard.BaseTest
{
    public abstract class PrivateBaseTest
    {
        protected bool ContinueOrderedTests { get; set; } = true;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            var thisType = GetType();

            PrintTestCaseIdAttribute(thisType);
        }

        public void PrintTestCaseIdAttribute(Type t)
        {
            var testCaseIdAttribute =
                (TestCaseIdAttribute)System.Attribute.GetCustomAttribute(t, typeof(TestCaseIdAttribute));

            if (testCaseIdAttribute != null)
            {
                var logger = Logger.MangoLogger.GetLogger();
                logger.StartTestCase(testCaseIdAttribute.TestCaseName);
            }
        }

        [ThreadStatic]
        protected static Logger.MangoLogger TestLogger;

        public virtual void SetUp()
        {
            // create test directory
            var testResultDirectory = GetTestResultDirectoryPath(out string testName, out string testArgsString);
            SetTestResultDirectory(testResultDirectory);

            TestLogger = Logger.MangoLogger.GetLogger();

            var hasDescriptionAttribute = TestContext.CurrentContext.Test.Properties.ContainsKey("Description");
            if (hasDescriptionAttribute)
            {
                var descriptions = TestContext.CurrentContext.Test.Properties["Description"].ToList();
                if (descriptions.Count > 0)
                    TestLogger.Info($"Descriptions:{descriptions.First()}");
            }
        }

        public virtual void TearDown()
        {
            var testName = $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.MethodName}";
            var testArgs = TestContext.CurrentContext.Test.Arguments.ToList();
            if (testArgs.Count > 0)
            {
                var testArgsString = string.Join(";", testArgs);
                TestLogger.AddStep($"Test Complete:{testName}({testArgsString})");
            }
            else
            {
                TestLogger.AddStep($"Test Complete:{testName}");
            }
        }

        protected void DocumentTestStatus()
        {
            var testFixtureName = TestContext.CurrentContext.Test.ClassName.Split(',').Last();
            var testName = TestContext.CurrentContext.Test.Name;
            var status = TestContext.CurrentContext.Result.Outcome.Status.ToString("g");
            var reason = TestContext.CurrentContext.Result.Message;
            var reasonNoBreak = reason?.Replace("\n", "").Replace("\r", "");

            PrerequisiteTests.SetStatus($"{testFixtureName}.{testName}", status, reasonNoBreak);

            if (ContinueOrderedTests)
                ContinueOrderedTests = status != "Failed";
        }

        public IWebDriver Driver { get; protected set; }

        private static string _assemblyPath;

        public static string AssemblyPath
        {
            get
            {
                if (string.IsNullOrEmpty(_assemblyPath))
                {
                    _assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }

                return _assemblyPath;
            }
        }

        [ThreadStatic]
        public static string TestResultDirectory;

        protected void SetTestResultDirectory(string testResultDirectory)
        {
            TestResultDirectory = testResultDirectory.Replace("|", "");
            Directory.CreateDirectory(testResultDirectory);
            CaptureTestResults.TestResultDirectory = TestResultDirectory;
        }

        protected string GetTestResultDirectoryPath(out string testName, out string testArgsString)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            var className = TestContext.CurrentContext.Test.ClassName.Split(',').Last();
            testName = TestContext.CurrentContext.Test.MethodName;

            var testArgs = TestContext.CurrentContext.Test.Arguments.ToList();
            testArgsString = null;
            string testResultDirectory = null;

            if (testArgs.Count > 0)
            {
                testArgsString = string.Join(";", testArgs);

                logger.Info($"Test Started: {testName}({testArgsString})");

                testResultDirectory = $@"{AppSettings.TestResultRootDirectory}\{className}\{testName}\{testArgsString}";
            }
            else
            {
                logger.Info($"Test Started: {testName}");

                testResultDirectory = $@"{AppSettings.TestResultRootDirectory}\{className}\{testName}";
            }

            return testResultDirectory;
        }
    }
}
