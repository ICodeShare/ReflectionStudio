using System;

namespace Infrastructure.Settings
{
	public class SettingsObject
	{
		private readonly SerializableDictionary<string, string> _customSettings;
		public int MessagingServerPort
		{
			get;
			set;
		}
		public string MessagingServerName
		{
			get;
			set;
		}
		public string TerminalName
		{
			get;
			set;
		}
        /// <summary>
        /// 用户认证终结点
        /// </summary>
        public string OAuthTokenEndPoint
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端认证Id
        /// </summary>
        public string OAuth2ClientId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端认证密码
        /// </summary>
        public string OAuth2ClientSecret
        {
            get;
            set;
        }
        /// <summary>
        /// 查询命令
        /// </summary>
        public string StrScopes
        {
            get;
            set;
        }
        /// <summary>
        /// 用户服务终结点
        /// </summary>
        public string UserServiceEndPoint
        {
            get;
            set;
        }


        /// <summary>
        /// 服务终结点
        /// </summary>
        public string ServiceEndPoint
        {
            get;
            set;
        }


		public string ConnectionString
		{
			get;
			set;
		}
		public bool StartMessagingClient
		{
			get;
			set;
		}
		public string LogoPath
		{
			get;
			set;
		}
		public string DefaultHtmlReportHeader
		{
			get;
			set;
		}
		public string CurrentLanguage
		{
			get;
			set;
		}
		public bool OverrideLanguage
		{
			get;
			set;
		}
		public bool OverrideWindowsRegionalSettings
		{
			get;
			set;
		}
		public int DefaultRecordLimit
		{
			get;
			set;
		}
		public double WindowScale
		{
			get;
			set;
		}
		public double FooterHeight
		{
			get;
			set;
		}
		public bool UseBoldFonts
		{
			get;
			set;
		}
		public bool ValidateExitButton
		{
			get;
			set;
		}
		public bool AllowMultipleClients
		{
			get;
			set;
		}
		public string CallerIdDeviceName
		{
			get;
			set;
		}
		public string ScaleDeviceName
		{
			get;
			set;
		}
		public string AdditionalDevices
		{
			get;
			set;
		}
		public bool AutoMigrate
		{
			get;
			set;
		}
		public string ApiHost
		{
			get;
			set;
		}
		public string ApiPort
		{
			get;
			set;
		}
		public TimeSpan TokenLifeTime
		{
			get;
			set;
		}
		public string PrintFontFamily
		{
			get;
			set;
		}
		public SerializableDictionary<string, string> CustomSettings
		{
			get
			{
				return this._customSettings;
			}
		}
		public SettingsObject()
		{
			this._customSettings = new SerializableDictionary<string, string>();
			this.AutoMigrate = true;
			this.MessagingServerPort = 8080;
            this.OAuthTokenEndPoint = "";
            this.OAuth2ClientId = "";
            this.OAuth2ClientSecret = "";
            this.StrScopes = "";
            this.ServiceEndPoint = "";
            this.UserServiceEndPoint = "";
			this.ConnectionString = "";
            this.DefaultHtmlReportHeader = "";
			//this.DefaultHtmlReportHeader = "\r\n<style type='text/css'> \r\nhtml\r\n{\r\n  font-family: 'Courier New', monospace;\r\n  font-size: 11px;\r\n} \r\n</style>";
		}
		public void SetCustomValue(string settingName, string settingValue)
		{
			if (!this.CustomSettings.ContainsKey(settingName))
			{
				this.CustomSettings.Add(settingName, settingValue);
			}
			else
			{
				this.CustomSettings[settingName] = settingValue;
			}
			if (string.IsNullOrEmpty(settingValue))
			{
				this.CustomSettings.Remove(settingName);
			}
		}
		public string GetCustomValue(string settingName)
		{
			if (!this.CustomSettings.ContainsKey(settingName))
			{
				return "";
			}
			return this.CustomSettings[settingName];
		}
	}
}
