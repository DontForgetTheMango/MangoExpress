using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using MangoExpressStandard.DTO;

namespace MangoExpressStandard.Util
{
	public class AppSettings
	{
        /// <summary>
		/// Get build version
		/// </summary>
		public static int BuildVersion { get; } = Get<int>("BuildVersion");

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:MangoExpressStandard.Util.AppSettings"/> is release.
        /// </summary>
        /// <value><c>true</c> if release; otherwise, <c>false</c>.</value>
		public static bool Release { get; } = true;

        #region TestDetails Info
        /// <summary>
        /// Only execute DTO.TestDetails when Execute == true.
        /// </summary>
        /// <value><c>true</c> if test details execute only; otherwise, <c>false</c>.</value>
        public static bool TestDetailsExecuteOnly => Get<bool>("TestDetailsExecuteOnly");
        #endregion
        #region Folder Info
        /// <summary>
        /// Gets the test result directory.
        /// </summary>
        /// <value>The test result directory.</value>
        public static string TestResultsDirectory { get; } = GetRequiredAppString("TestResultsDirectory");
        /// <summary>
        /// Gets the test result directory.
        /// </summary>
        /// <value>The test result directory.</value>
        public static string TestDetailsDirectory { get; } = GetRequiredAppString("TestDetailsDirectory");

        /// <summary>
        /// Gets the downloads root directory.
        /// </summary>
        /// <value>The downloads root directory.</value>
		public static string DownloadsDirectory { get; } = GetRequiredAppString("DownloadsDirectory");
        #endregion
        #region Browser Info
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

        /// <summary>
        /// Gets the size of the browser.
        /// </summary>
        /// <value>The size of the browser.</value>
        public static BrowserSize BrowserSize { get; } = new BrowserSize(Get<string>("BrowserSize"));

        public static bool HeadlessMode { get; } = Get<bool>("HeadlessMode");
        #endregion

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
