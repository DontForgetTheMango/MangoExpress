using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using static IQ.Test.Utils.WebDriverFactory;

namespace MangoExpress.Utilities
{
	public static class AppSettings
	{
		public static T Get<T>(string key, bool checkAppSettingsOverrideFile = true)
		{
			string appSetting = null;
			
			if (checkAppSettingsOverrideFile)
			{
				appSetting = GetAppStringFromFile(key) ?? ConfigurationManager.AppSettings[key];
			}
			else{
				appSetting = ConfigurationManager.AppSettings[key];
			}
			
			if (string.IsNullOrWhiteSpace(appSetting)) return default(T);
			
			var convert = TypeDescriptor.GetConverter(typeof(T));
			return (T)(convert.ConvertFromInvariantString(appSetting));
		}
		
		public static string GetAppString(string appSetting){
			var retVall = AppSettings.Get<string>(appSetting);
			
			if (string.IsNullOrEmpty(retVal)){
				throw new ConfigurationErrorsException($"'{appSettings}' is not set in app.config!");
			}
		}
		
		public static string BuildVersion {get;} = AppSetting.Get<string>("BuildVersion");
	}
}
