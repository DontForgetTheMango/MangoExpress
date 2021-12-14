using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MangoExpressStandard.BaseTest;
using MangoExpressStandard.Extension;

namespace MangoExpressStandard.Util
{
    /// <summary>
    /// Get test versions and test pages from 
    /// test.default.config and test.override.config
    /// in assembly path
    /// </summary>
    public class TestSettings
    {
        /// <summary>
        /// Example for implementing testVersion in test.default.config
        /// default value is -1 if "ExampleTestVersion not found in config
        /// </summary>
        /// <value>The example test version.</value>
        public static int ExampleTestVersion { get; } = GetTestVersion("ExampleTestVersion");

        /// <summary>
        /// Example for implementing pageVersion in test.devault.config
        /// default value is -1 if "ExamplePageVersion" not found in config
        /// </summary>
        /// <value>The example page version.</value>
        public static int ExamplePageVersion { get; } = GetPageVersion("ExamplePageVersion");

        private static Dictionary<string, Dictionary<string, int>> _versions
            = new Dictionary<string, Dictionary<string, int>>();

        public static int GetTestVersion(string test)
        {
            if (_versions.IsNullOrEmpty())
                PopulateVersions();

            if (_versions.TryGetValue("test", out Dictionary<string, int> testVersions))
            {
                if (testVersions.TryGetValue(test, out var value))
                {
                    return value;
                }
            }

            return -1;
        }

        public static int GetPageVersion(string page)
        {
            if (_versions.IsNullOrEmpty())
                PopulateVersions();

            if (_versions.TryGetValue("page", out Dictionary<string, int> pageVersions))
            {
                if (pageVersions.TryGetValue(page, out var value))
                {
                    return value;
                }
            }

            return -1;
        }

        private static void PopulateVersions()
        {
            var logger = Logger.MangoLogger.GetLogger();

            var assemblyPath = PrivateBaseTest.AssemblyPath;

            var defaultTestConfigFilePath = $@"{assemblyPath}/test.default.config";
            if (File.Exists(defaultTestConfigFilePath))
                UpdateTestVersions(defaultTestConfigFilePath);

            var overrideTestConfigFilePath = $@"{assemblyPath}/test.override.config";
            if (File.Exists(overrideTestConfigFilePath))
                UpdateTestVersions(overrideTestConfigFilePath);

            foreach (var version in _versions)
            {
                logger.Info(version.Key);
                foreach (var v in version.Value)
                {
                    logger.Info($"\t{v.Key} -> {v.Value}");
                }
            }
        }

        private static void UpdateTestVersions(string configFile)
        {
            var doc = new XmlDocument();
            try
            {
                doc.Load(configFile);

                UpdateVersions(doc, "test");

                UpdateVersions(doc, "page");
            }
            catch (Exception ex)
            {
                var logger = Logger.MangoLogger.GetLogger();
                logger.Warn($"{configFile} configured wrong!");
                logger.Warn(ex.Message);
            }
        }

        private static void UpdateVersions(XmlDocument document, string node)
        {
            var versionNodes = document.DocumentElement?.SelectNodes($"/versions/{node}Version/{node}Version");
            if (versionNodes != null)
            {
                if (!_versions.ContainsKey(node))
                    _versions.Add(node, new Dictionary<string, int>());

                foreach (XmlNode versionNode in versionNodes)
                {
                    if (versionNode.Attributes != null)
                    {
                        var testName = versionNode.Attributes[node].Value;
                        var testVersion = versionNode.Attributes[node].Value;

                        var isValidVersion = int.TryParse(testVersion, out int testVersionInt);

                        if (!isValidVersion && (testVersion.ToLower() == "true" || testVersion.ToLower() == "false"))
                        {
                            testVersionInt = (testVersion.ToLower() == "true") ? 1 : 0;
                            isValidVersion = true;
                        }

                        if (isValidVersion)
                        {
                            if (_versions[node].ContainsKey(testName))
                                _versions[node][testName] = testVersionInt;
                            else
                                _versions[node].Add(testName, testVersionInt);
                        }
                    }
                }
            }
        }
    }
}
