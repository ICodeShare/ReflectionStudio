using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
namespace Infrastructure.Settings
{
	public static class LocalSettings
	{
		private static SettingsObject _settingsObject;
		private static CultureInfo _cultureInfo;
		private static string _databaseName;
		private static IList<string> _supportedLanguages;
		private static Dictionary<string, string> _versionData;
		private static readonly string VersionDataFilePath;
        
		public static int Decimals
		{
			get
			{
				return 2;
			}
		}
		public static bool AllowMultipleClients
		{
			get
			{
				return LocalSettings._settingsObject.AllowMultipleClients;
			}
			set
			{
				LocalSettings._settingsObject.AllowMultipleClients = value;
			}
		}
		public static int MessagingServerPort
		{
			get
			{
				return LocalSettings._settingsObject.MessagingServerPort;
			}
			set
			{
				LocalSettings._settingsObject.MessagingServerPort = value;
			}
		}
		public static string MessagingServerName
		{
			get
			{
				return LocalSettings._settingsObject.MessagingServerName;
			}
			set
			{
				LocalSettings._settingsObject.MessagingServerName = value;
			}
		}
		public static string TerminalName
		{
			get
			{
				return LocalSettings._settingsObject.TerminalName;
			}
			set
			{
				LocalSettings._settingsObject.TerminalName = value;
			}
		}
        /// <summary>
        /// 用户认证终结点
        /// </summary>
        public static string OAuthTokenEndPoint
        {
            get
            {
                return LocalSettings._settingsObject.OAuthTokenEndPoint;
            }
            set
            {
                LocalSettings._settingsObject.OAuthTokenEndPoint = value;
            }
        }
        /// <summary>
        /// 客户端认证Id
        /// </summary>
        public static string OAuth2ClientId
        {
            get
            {
                return LocalSettings._settingsObject.OAuth2ClientId;
            }
            set
            {
                LocalSettings._settingsObject.OAuth2ClientId = value;
            }
        }
        /// <summary>
        /// 客户端认证密码
        /// </summary>
        public static string OAuth2ClientSecret
        {
            get
            {
                return LocalSettings._settingsObject.OAuth2ClientSecret;
            }
            set
            {
                LocalSettings._settingsObject.OAuth2ClientSecret = value;
            }
        }
        /// <summary>
        /// 查询命令
        /// </summary>
        public static string StrScopes
        {
            get
            {
                return LocalSettings._settingsObject.StrScopes;
            }
            set
            {
                LocalSettings._settingsObject.StrScopes = value;
            }
        }

        /// <summary>
        /// 用户服务终结点
        /// </summary>
        public static string UserServiceEndPoint
        {
            get
            {
                return LocalSettings._settingsObject.UserServiceEndPoint;
            }
            set
            {
                LocalSettings._settingsObject.UserServiceEndPoint = value;
            }
        }

        /// <summary>
        /// 服务终结点
        /// </summary>
        public static string ServiceEndPoint
        {
            get
            {
                return LocalSettings._settingsObject.ServiceEndPoint;
            }
            set
            {
                LocalSettings._settingsObject.ServiceEndPoint = value;
            }
        }
		public static string ConnectionString
		{
			get
			{
				return LocalSettings._settingsObject.ConnectionString;
			}
			set
			{
				LocalSettings._settingsObject.ConnectionString = value;
			}
		}
		public static bool StartMessagingClient
		{
			get
			{
				return LocalSettings._settingsObject.StartMessagingClient;
			}
			set
			{
				LocalSettings._settingsObject.StartMessagingClient = value;
			}
		}
		public static string LogoPath
		{
			get
			{
				return LocalSettings._settingsObject.LogoPath;
			}
			set
			{
				LocalSettings._settingsObject.LogoPath = value;
			}
		}
		public static string PrintFontFamily
		{
			get
			{
				if (string.IsNullOrEmpty(LocalSettings._settingsObject.PrintFontFamily) || LocalSettings._settingsObject.PrintFontFamily == "")
				{
					LocalSettings._settingsObject.PrintFontFamily = "Courier New";
					LocalSettings.SaveSettings();
				}
				return LocalSettings._settingsObject.PrintFontFamily;
			}
			set
			{
				LocalSettings._settingsObject.PrintFontFamily = value;
				LocalSettings.SaveSettings();
			}
		}
		public static string ApiHost
		{
			get
			{
				return LocalSettings._settingsObject.ApiHost;
			}
			set
			{
				LocalSettings._settingsObject.ApiHost = value;
				LocalSettings.SaveSettings();
			}
		}
		public static string ApiPort
		{
			get
			{
				return LocalSettings._settingsObject.ApiPort;
			}
			set
			{
				LocalSettings._settingsObject.ApiPort = value;
				LocalSettings.SaveSettings();
			}
		}
		public static string DefaultHtmlReportHeader
		{
			get
			{
				return LocalSettings._settingsObject.DefaultHtmlReportHeader;
			}
			set
			{
				LocalSettings._settingsObject.DefaultHtmlReportHeader = value;
			}
		}
		public static string CurrentLanguage
		{
			get
			{
				return LocalSettings._settingsObject.CurrentLanguage;
			}
			set
			{
				LocalSettings._settingsObject.CurrentLanguage = value;
				LocalSettings._cultureInfo = CultureInfo.GetCultureInfo(value);
				LocalSettings.UpdateThreadLanguage();
				LocalSettings.SaveSettings();
			}
		}
		public static bool OverrideWindowsRegionalSettings
		{
			get
			{
				return LocalSettings._settingsObject.OverrideWindowsRegionalSettings;
			}
			set
			{
				LocalSettings._settingsObject.OverrideWindowsRegionalSettings = value;
			}
		}
		public static int DefaultRecordLimit
		{
			get
			{
				return LocalSettings._settingsObject.DefaultRecordLimit;
			}
			set
			{
				LocalSettings._settingsObject.DefaultRecordLimit = value;
			}
		}
		public static double WindowScale
		{
			get
			{
				return LocalSettings._settingsObject.WindowScale;
			}
			set
			{
				LocalSettings._settingsObject.WindowScale = value;
			}
		}
		public static double FooterHeight
		{
			get
			{
				return LocalSettings._settingsObject.FooterHeight;
			}
			set
			{
				LocalSettings._settingsObject.FooterHeight = value;
			}
		}
		public static bool UseBoldFonts
		{
			get
			{
				return LocalSettings._settingsObject.UseBoldFonts;
			}
			set
			{
				LocalSettings._settingsObject.UseBoldFonts = value;
			}
		}
		public static bool ValidateExitButton
		{
			get
			{
				return LocalSettings._settingsObject.ValidateExitButton;
			}
			set
			{
				LocalSettings._settingsObject.ValidateExitButton = value;
			}
		}
		public static string CallerIdDeviceName
		{
			get
			{
				return LocalSettings._settingsObject.CallerIdDeviceName;
			}
			set
			{
				LocalSettings._settingsObject.CallerIdDeviceName = value;
			}
		}
		public static string ScaleDeviceName
		{
			get
			{
				return LocalSettings._settingsObject.ScaleDeviceName;
			}
			set
			{
				LocalSettings._settingsObject.ScaleDeviceName = value;
			}
		}
		public static string AdditionalDevices
		{
			get
			{
				return LocalSettings._settingsObject.AdditionalDevices;
			}
			set
			{
				LocalSettings._settingsObject.AdditionalDevices = value;
			}
		}
		public static bool AutoMigrate
		{
			get
			{
				return LocalSettings._settingsObject.AutoMigrate;
			}
			set
			{
				LocalSettings._settingsObject.AutoMigrate = value;
			}
		}
		public static string AppPath
		{
			get;
			set;
		}
		public static string DocumentPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\" + LocalSettings.AppName;
			}
		}
		public static string AddonPath
		{
			get
			{
				return LocalSettings.DocumentPath + "\\Addons";
			}
		}
		public static string LocalAddonPath
		{
			get
			{
				return LocalSettings.AppPath + "\\Addons";
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public static string DataPath
		{
			get
			{
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\SecurityFire\\" + LocalSettings.AppName;
			}
		}
		public static string UserPath
		{
			get
			{
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SecurityFire\\" + LocalSettings.AppName;
			}
		}
		public static string CommonSettingsFileName
		{
			get
			{
                return LocalSettings.DataPath + "\\SecurityFireSettings.txt";
			}
		}
		public static string UserSettingsFileName
		{
			get
			{
                return LocalSettings.UserPath + "\\SecurityFireSettings.txt";
			}
		}
		public static string SettingsFileName
		{
			get
			{
				if (!File.Exists(LocalSettings.UserSettingsFileName))
				{
					return LocalSettings.CommonSettingsFileName;
				}
				return LocalSettings.UserSettingsFileName;
			}
		}
		public static string CurrencyFormat
		{
			get;
			set;
		}
		public static string QuantityFormat
		{
			get;
			set;
		}
		public static string ReportCurrencyFormat
		{
			get;
			set;
		}
		public static string ReportQuantityFormat
		{
			get;
			set;
		}
		public static string PrintoutCurrencyFormat
		{
			get;
			set;
		}
		public static string CurrencySymbol
		{
			get
			{
				return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
			}
		}
		private static int DefaultDbVersion
		{
			get
			{
				return 78;
			}
		}
		private static string DefaultAppVersion
		{
			get
			{
				return "4.1.82";
			}
		}
		public static int DbVersion
		{
			get
			{
				if (!LocalSettings.CanReadVersionFromFile())
				{
					return LocalSettings.DefaultDbVersion;
				}
				return Convert.ToInt32(LocalSettings.GetVersionDat("DbVersion"));
			}
		}
		public static string AppVersion
		{
			get
			{
				if (!LocalSettings.CanReadVersionFromFile())
				{
					return LocalSettings.DefaultAppVersion;
				}
				return LocalSettings.GetVersionDat("AppVersion");
			}
		}
		public static DateTime AppVersionDateTime
		{
			get
			{
				if (!LocalSettings.CanReadVersionFromFile())
				{
					return DateTime.Now;
				}
				Regex regex = new Regex("(\\d\\d\\d\\d)-(\\d\\d)-(\\d\\d) (\\d\\d)(\\d\\d)");
				Match match = regex.Match(LocalSettings.GetVersionDat("VersionTime"));
				return new DateTime(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value), Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value), Convert.ToInt32(match.Groups[5].Value), 0);
			}
		}
		public static string AppName
		{
			get
			{
                return "SecurityFire3";
			}
		}
		public static string DatabaseName
		{
			get
			{
				return LocalSettings._databaseName ?? LocalSettings.AppName;
			}
			set
			{
				LocalSettings._databaseName = value;
			}
		}
		public static IList<string> SupportedLanguages
		{
			get
			{
				IList<string> arg_1F_0;
				if ((arg_1F_0 = LocalSettings._supportedLanguages) == null)
				{
					arg_1F_0 = (LocalSettings._supportedLanguages = new string[]
					{
						"en"
					});
				}
				return arg_1F_0;
			}
		}
		public static long CurrentDbVersion
		{
			get;
			set;
		}
		public static string DatabaseLabel
		{
			get
			{
				if (LocalSettings.ConnectionString.ToLower().Contains(".sdf"))
				{
					return "CE";
				}
				if (LocalSettings.ConnectionString.ToLower().Contains(".mdf"))
				{
					return "LD";
				}
				if (LocalSettings.ConnectionString.ToLower().Contains("data source"))
				{
					return "SQ";
				}
				if (LocalSettings.ConnectionString.ToLower().StartsWith("mongodb://"))
				{
					return "MG";
				}
				if (string.IsNullOrEmpty(LocalSettings.ConnectionString) && LocalSettings.IsSqlce40Installed())
				{
					return "CE";
				}
				if (string.IsNullOrEmpty(LocalSettings.ConnectionString) && LocalSettings.IsSqlLocalDbInstalled())
				{
					return "SQ";
				}
				return "TX";
			}
		}
		
		private static bool CanReadVersionFromFile()
		{
			return File.Exists(LocalSettings.VersionDataFilePath);
		}
		private static string GetVersionDat(string versionType)
		{
			if (LocalSettings._versionData == null || LocalSettings._versionData.Count == 0)
			{
				LocalSettings._versionData = new Dictionary<string, string>();
				string[] array = File.ReadAllLines(LocalSettings.VersionDataFilePath);
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					string[] array2 = text.Split(new char[]
					{
						'='
					});
					LocalSettings._versionData.Add(array2[0], array2[1]);
				}
			}
			if (LocalSettings._versionData.ContainsKey(versionType))
			{
				return LocalSettings._versionData[versionType];
			}
			throw new ArgumentOutOfRangeException("versionType", "VersionType " + versionType + " doesn't exist!");
		}
		public static bool IsSqlce40Installed()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SQL Server Compact Edition\\v4.0");
			return registryKey != null;
		}
		public static bool IsSqlLocalDbInstalled()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SQL Server Local DB\\Installed Versions\\11.0");
			return registryKey != null;
		}
		public static void SaveSettings()
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(LocalSettings._settingsObject.GetType());
				XmlTextWriter xmlTextWriter = new XmlTextWriter(LocalSettings.SettingsFileName, null);
				try
				{
					xmlSerializer.Serialize(xmlTextWriter, LocalSettings._settingsObject);
				}
				finally
				{
					xmlTextWriter.Close();
				}
			}
			catch (UnauthorizedAccessException)
			{
				if (!File.Exists(LocalSettings.UserSettingsFileName))
				{
					File.Create(LocalSettings.UserSettingsFileName).Close();
					LocalSettings.SaveSettings();
				}
			}
			catch (IOException)
			{
			}
		}
		public static void LoadSettings()
		{
			LocalSettings._settingsObject = new SettingsObject();
			string settingsFileName = LocalSettings.SettingsFileName;
			string text = Path.Combine(Path.GetDirectoryName(LocalSettings.SettingsFileName), Path.GetFileNameWithoutExtension(LocalSettings.SettingsFileName) + "_Backup.txt");
			if (File.Exists(settingsFileName))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(LocalSettings._settingsObject.GetType());
				XmlTextReader xmlTextReader = new XmlTextReader(settingsFileName);
				try
				{
					LocalSettings._settingsObject = (xmlSerializer.Deserialize(xmlTextReader) as SettingsObject);
				}
				catch
				{
					xmlTextReader.Close();
					if (File.Exists(text))
					{
						File.Delete(settingsFileName);
						File.Copy(text, settingsFileName);
						File.Delete(text);
						LocalSettings.LoadSettings();
						return;
					}
				}
				xmlTextReader.Close();
				try
				{
					File.Copy(settingsFileName, text, true);
				}
				catch (Exception)
				{
				}
			}
			if (LocalSettings.DefaultRecordLimit == 0)
			{
				LocalSettings.DefaultRecordLimit = 100;
			}
		}
		static LocalSettings()
		{
			LocalSettings.VersionDataFilePath = LocalSettings.DataPath + "\\version.dat";
			if (!Directory.Exists(LocalSettings.DocumentPath))
			{
				Directory.CreateDirectory(LocalSettings.DocumentPath);
			}
			if (!Directory.Exists(LocalSettings.AddonPath))
			{
				Directory.CreateDirectory(LocalSettings.AddonPath);
			}
			if (!Directory.Exists(LocalSettings.DataPath))
			{
				Directory.CreateDirectory(LocalSettings.DataPath);
			}
			if (!Directory.Exists(LocalSettings.UserPath))
			{
				Directory.CreateDirectory(LocalSettings.UserPath);
			}
			LocalSettings.LoadSettings();
		}
		public static void UpdateThreadLanguage()
		{
			if (LocalSettings._cultureInfo != null)
			{
				if (LocalSettings.OverrideWindowsRegionalSettings)
				{
					Thread.CurrentThread.CurrentCulture = LocalSettings._cultureInfo;
				}
				Thread.CurrentThread.CurrentUICulture = LocalSettings._cultureInfo;
			}
		}
		public static void UpdateSetting(string settingName, string settingValue)
		{
			LocalSettings._settingsObject.SetCustomValue(settingName, settingValue);
			LocalSettings.SaveSettings();
		}
		public static string ReadSetting(string settingName)
		{
			return LocalSettings._settingsObject.GetCustomValue(settingName);
		}
		public static string LocateAddonFileName(string fileName)
		{
			string text = LocalSettings.LocalAddonPath + "\\" + fileName;
			if (!File.Exists(text))
			{
				text = LocalSettings.AppPath + "\\" + fileName;
			}
			if (!File.Exists(text))
			{
				return "";
			}
			return text;
		}

      
	}
}
