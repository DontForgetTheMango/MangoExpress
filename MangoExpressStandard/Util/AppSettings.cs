using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using MangoExpressStandard.DTO;

namespace MangoExpressStandard.Util
{
	public class AppSettings
	{
        /// <summary>
        /// Example for implementing testVersion in test.default.config
        /// default value is -1 if "ExampleTestVersion not found in config
        /// </summary>
        /// <value>The example test version.</value>
        public static int ExampleTestVersion { get; } = TestSettings.GetTestVersion("ExampleTestVersion");

        /// <summary>
        /// Example for implementing pageVersion in test.devault.config
        /// default value is -1 if "ExamplePageVersion" not found in config
        /// </summary>
        /// <value>The example page version.</value>
        public static int ExamplePageVersion { get; } = TestSettings.GetPageVersion("ExamplePageVersion");

        /// <summary>
		/// Get build version
		/// </summary>
		public static string BuildVersion { get; } = GetRequiredAppString("BuildVersion");

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MangoExpressStandard.Util.AppSettings"/> is release.
        /// </summary>
        /// <value><c>true</c> if release; otherwise, <c>false</c>.</value>
		public static bool Release { get; } = true;

        /// <summary>
        /// Gets the size of the browser.
        /// </summary>
        /// <value>The size of the browser.</value>
        public static BrowserSize BrowserSize { get; } = new BrowserSize(Get<string>("BrowserSize"));

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MangoExpressStandard.Util.AppSettings"/> test details
        /// execute only.
        /// </summary>
        /// <value><c>true</c> if test details execute only; otherwise, <c>false</c>.</value>
        public static bool TestDetailsExecuteOnly => Get<bool>("TestDetailsExecuteOnly");
        /// <summary>
        /// Gets the test result directory.
        /// </summary>
        /// <value>The test result directory.</value>
		public static string TestResultDirectory { get; } = GetRequiredAppString("TestResultDirectory");

        /// <summary>
        /// Gets the test result root directory.
        /// </summary>
        /// <value>The test result root directory.</value>
        public static string TestResultRootDirectory { get; } = GetRequiredAppString("output");

        /// <summary>
        /// Gets the downloads root directory.
        /// </summary>
        /// <value>The downloads root directory.</value>
		public static string DownloadsRootDirectory { get; } = GetRequiredAppString("DownloadsRootDirectory");

        /// <summary>
        /// Gets the browser.
        /// </summary>
        /// <value>The browser.</value>
		public static BrowserOptions Browser
        {
			get
            {
				var browser = GetRequiredAppString("browser");

                switch (browser.ToLower())
                {
					case "chrome":
						return BrowserOptions.Chrome;
					case "ie":
						return BrowserOptions.IE;
					case "firefox":
						return BrowserOptions.FireFox;
					default:
						return default(BrowserOptions);
				}
            }
        }

        public enum BrowserOptions
        {
            Chrome,
            IE,
            FireFox
        }

        public static bool HeadlessMode { get; } = Get<bool>("HeadlessMode");

        private static T Get<T>(string key)
        {
            string appSetting = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(appSetting)) return default(T);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }

        private static string GetRequiredAppString(string appSetting)
        {
            var retVall = Get<string>(appSetting);
            if (string.IsNullOrEmpty(retVall))
            {
                throw new ConfigurationErrorsException($"'{appSetting}' is not set in app.config!");
            }

            return retVall;
        }
	}
}
