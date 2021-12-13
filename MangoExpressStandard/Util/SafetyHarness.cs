using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;

namespace MangoExpressStandard.Util
{
    public static class SafetyHarness
    {
        public static TimeSpan DefaultMaximumWait = TimeSpan.FromSeconds(5);

        public static TimeSpan DefaultRetryDelay = TimeSpan.FromMilliseconds(100);

        public static List<Type> _defaultExceptionTypesToMonitor = new List<Type>()
        {
            typeof(StaleElementReferenceException)
        };

        public static void DefaultHandleException(Type exceptionType)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException(nameof(exceptionType));
            }

            if (!typeof(Exception).IsAssignableFrom(exceptionType))
            {
                throw new ArgumentException($"{exceptionType.FullName} is not an exception.", nameof(exceptionType));
            }

            _defaultExceptionTypesToMonitor.Add(exceptionType);
        }

        public static void Retry(
            Action actionToRetry,
            TimeSpan? retryDelay = null,
            TimeSpan? maximumWait = null,
            params Type[] exceptionsToMonitor)
        {
            if (actionToRetry == null)
            {
                throw new ArgumentNullException(nameof(actionToRetry));
            }

            var actualMaximumWait = maximumWait ?? DefaultMaximumWait;
            var actualRetryDelay = retryDelay ?? DefaultRetryDelay;
            var actualExceptionsToRetry = exceptionsToMonitor != null && exceptionsToMonitor.Length > 0
                ? exceptionsToMonitor
                : _defaultExceptionTypesToMonitor.ToArray();

            if (actualMaximumWait <= TimeSpan.Zero)
                throw new ArgumentException($"Timespan must be greater than 0.", nameof(actualMaximumWait));

            if (actualRetryDelay <= TimeSpan.Zero)
                throw new ArgumentException($"Timespan must be greater than 0.", nameof(actualRetryDelay));

            var timeRunning = Stopwatch.StartNew();

            while (true)
            {
                try
                {
                    actionToRetry();
                    return;
                }
                catch (Exception ex)
                {
                    if (timeRunning.Elapsed > actualMaximumWait)
                        throw;

                    if (!ShouldSquash(ex, actualExceptionsToRetry))
                        throw;

                    Thread.Sleep(actualRetryDelay);
                }
            }
        }

        public static TResult Retry<TResult>(
            Func<TResult> actionToRetry,
            TimeSpan? retryDelay = null,
            TimeSpan? maximumWait = null,
            params Type[] exceptionsToMonitor)
        {
            TResult result = default(TResult);

            Retry(() =>
            {
                result = actionToRetry();
            }, retryDelay, maximumWait, exceptionsToMonitor);

            return result;
        }

        private static bool ShouldSquash(Exception caught, Type[] exceptionTypesToSquash)
        {
            if (caught is AggregateException aggregate)
            {
                var squashed = aggregate.Flatten();
                foreach(var inner in squashed.InnerExceptions)
                {
                    if (ShouldSquash(inner, exceptionTypesToSquash))
                        return true;
                }

                return false;
            }

            var exceptionType = caught.GetType();
            foreach (var squashedType in exceptionTypesToSquash)
            {
                if (squashedType.IsAssignableFrom(exceptionType))
                    return true;
            }

            return false;
        }
    }
}
