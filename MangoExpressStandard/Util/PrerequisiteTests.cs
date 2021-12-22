using System;
using System.IO;
using System.Linq;
using MangoExpressStandard.BaseTest;
using NLog;
using NUnit.Framework;

namespace MangoExpressStandard.Util
{
    public static class PrerequisiteTests
    {
        public static void SetStatus(string key, string status, string reason)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug($"Set test status {key}:{status}");

            var file = $@"{PrivateBaseTest.AssemblyPath}/TestStatus.txt";
            var lines = File.ReadLines(file).ToList();

            int i = 1;
            int lineNumber = 0;
            foreach(var line in lines)
            {
                if (line.StartsWith($"{key}:"))
                {
                    lineNumber = i;
                    break;
                }
                i++;
            }

            var newLineText = $"{key}:{status}:{reason}" + Environment.NewLine;

            if (lineNumber > 0)
            {
                lines[lineNumber - 1] = newLineText;
                File.WriteAllLines(file, lines);
            }
            else
            {
                File.AppendAllText(file, newLineText);
            }
        }

        private static string GetStatus(string key)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var file = $@"{PrivateBaseTest.AssemblyPath}/TestStatus.txt";
            var lines = File.ReadLines(file).ToList();
            foreach(var line in lines)
            {
                if (line.StartsWith($"{key}:"))
                {
                    return line.Split(':')[1];
                }
            }
            return null;
        }

        public static void IgnoreIfNotPassed(params string[] testNames)
        {
            var logger = LogManager.GetCurrentClassLogger();

            foreach (var testName in testNames)
            {
                var status = GetStatus(testName);
                if  (string.IsNullOrEmpty(status) || status != "Passed")
                {
                    logger.Info($"Prerequisite Test '{testName}' did not pass!");
                    Assert.Ignore($"Prerequisite Test '{testName}' did not pass!");
                }
            }
        }
    }
}
