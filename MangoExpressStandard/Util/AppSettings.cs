using System.ComponentModel;
using System.Configuration;

namespace MangoExpressStandard.Util
{
	public static class AppSettings
	{
		private static T Get<T>(string key)
		{
			var appSetting = ConfigurationManager.AppSettings[key];

			if (string.IsNullOrWhiteSpace(appSetting)) return default;

			var convert = TypeDescriptor.GetConverter(typeof(T));
			return (T)(convert.ConvertFromInvariantString(appSetting));
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

		public static bool Release { get; } = true;

		public static string TestResultDirectory { get; } = GetRequiredAppString("TestResultDirectory");

		public static string DownloadsRootDirectory { get; } = GetRequiredAppString("DownloadsRootDirectory");

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
