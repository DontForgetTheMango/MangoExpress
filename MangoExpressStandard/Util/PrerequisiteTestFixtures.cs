using System;
using System.IO;
using System.Linq;
using MangoExpressStandard.BaseTest;
using NLog;
using NUnit.Framework;

namespace MangoExpressStandard.Util
{
    public static class PrerequisiteTestFixtures
    {
        public static void SetStatus(string key, string status)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug($"Set test fixture status {key}:{status}");

            File.AppendAllText(
                $@"{PrivateBaseTest.AssemblyPath}/TestFixtureStatus.txt", 
                $"{key}:{status}" + Environment.NewLine);
        }

        private static string GetStatus(string key)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var file = $@"{PrivateBaseTest.AssemblyPath}/TestFixtureStatus.txt";
            var lines = File.ReadLines(file).ToList();
            foreach (var line in lines)
            {
                if (line.StartsWith($"{key}:"))
                {
                    return line.Split(':')[1];
                }
            }
            return null;
        }

        public static void IgnoreIfFailed(params string[] testNames)
        {
            var logger = LogManager.GetCurrentClassLogger();

            foreach (var testName in testNames)
            {
                var status = GetStatus(testName);
                if (string.IsNullOrEmpty(status) || status == "Failed")
                {
                    logger.Info($"Prerequisite Test '{testName}' failed!");
                    Assert.Ignore($"Prerequisite Test '{testName}' failed!");
                }
            }
        }
    }
}
