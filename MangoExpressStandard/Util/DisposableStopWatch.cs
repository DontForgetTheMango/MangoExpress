using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NLog;

namespace MangoExpressStandard.Util
{
    public class DisposableStopWatch : IDisposable
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private string _logMessage;
        private LogLevel _logLevel;

        public DisposableStopWatch(LogLevel logLevel, [CallerMemberName] string logMessage = "Unknown")
        {
            _logMessage = logMessage;
            _logLevel = logLevel;

            if (logMessage != null)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Log(_logLevel, $"{_logMessage}:start time:{DateTime.Now.ToString("mm':'ss':'fff")}");
            }

            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            if (_logMessage != null)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Log(_logLevel, $"{_logMessage}:elapsed time:{Elapsed.ToString("mm':'ss':'fff")}");
            }
        }

        public TimeSpan Elapsed => _stopwatch.Elapsed;
    }
}
