using System;
using NLog;

namespace MangoExpressStandard.Logger
{
    public class MangoLogger
    {
        [ThreadStatic]
        private static MangoLogger _mangoLogger;

        public static MangoLogger GetLogger()
        {
            return _mangoLogger ?? (_mangoLogger = new MangoLogger());
        }

        protected NLog.Logger StepLogger { get; } = LogManager.GetLogger("steplogger");

        public void AddStep(string step)
        {
            StepLogger.Info(step);
            TestCaseLogger.Info(step);
        }

        #region Assert Logger
        protected NLog.Logger AssertLogger { get; } = LogManager.GetLogger("assertlogger");

        public void AddAssert(string assertMessage)
        {
            AssertLogger.Info(assertMessage);
        }
        #endregion
        #region Console Logger
        protected NLog.Logger ConsoleLogger { get; } = LogManager.GetLogger("consolelogger");

        public void Fatal(string message)
        {
            ConsoleLogger.Fatal(message);
        }

        public void Error(string message)
        {
            ConsoleLogger.Error(message);
        }

        public void Warn(string message)
        {
            ConsoleLogger.Warn(message);
        }

        public void Info(string message)
        {
            ConsoleLogger.Info(message);
        }

        public void Debug(string message)
        {
            ConsoleLogger.Debug(message);
        }

        public void Trace(string message)
        {
            ConsoleLogger.Trace(message);
        }
        #endregion

        #region
        protected NLog.Logger TestCaseLogger { get; } = LogManager.GetLogger("testcaselogger");

        public void StartTestCase(string testCaseId)
        {
            TestCaseLogger.Info(testCaseId);
        }

        public void EndTestCase(string testCaseId)
        {
            TestCaseLogger.Info(testCaseId);
        }
        #endregion
    }
