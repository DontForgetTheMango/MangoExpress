using System.ComponentModel;
using System.Configuration;

namespace MangoExpressStandard.Util
{
	public class AppSettings
	{
		private static T Get<T>(string key)
		{
            string appSetting = ConfigurationManager.AppSettings[key];

			if (string.IsNullOrWhiteSpace(appSetting)) return default(T);

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
        /// Gets the downloads root directory.
        /// </summary>
        /// <value>The downloads root directory.</value>
		public static string DownloadsRootDirectory { get; } = GetRequiredAppString("DownloadsRootDirectory");

        /// <summary>
        /// Gets the browser.
        /// </summary>
        /// <value>The browser.</value>
		public static WebDriverFactory.BrowserOptions Browser
        {
			get
            {
				var browser = GetRequiredAppString("browser");

                switch (browser.ToLower())
                {
					case "chrome":
						return WebDriverFactory.BrowserOptions.Chrome;
					case "ie":
						return WebDriverFactory.BrowserOptions.IE;
					case "firefox":
						return WebDriverFactory.BrowserOptions.FireFox;
					default:
						return default(WebDriverFactory.BrowserOptions);
				}
            }
        }

		public static bool HeadlessMode { get; } = Get<bool>("HeadlessMode");
	}
}
