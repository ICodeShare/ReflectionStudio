using System;
using System.Globalization;
using System.Reflection;
namespace Localization.Engine
{
	public static class LocalizedObjectOperation
	{
		public static string GetErrorMessage(int errorNo)
		{
			string result;
			try
			{
				result = (string)LocalizeDictionary.Instance.GetLocalizedObject<object>(LocalizeDictionary.Instance.GetAssemblyName(Assembly.GetExecutingAssembly()), "ResError", "ERR_" + errorNo, LocalizeDictionary.Instance.Culture);
			}
			catch
			{
				result = "No localized ErrorMessage founded for ErrorNr: " + errorNo;
			}
			return result;
		}
		public static string GetGuiString(string key)
		{
			return LocalizedObjectOperation.GetGuiString(key, LocalizeDictionary.Instance.Culture);
		}
		public static string GetGuiString(string key, CultureInfo language)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key == string.Empty)
			{
				throw new ArgumentException("key is empty", "key");
			}
			string result;
			try
			{
				result = (string)LocalizeDictionary.Instance.GetLocalizedObject<object>(LocalizeDictionary.Instance.GetAssemblyName(Assembly.GetExecutingAssembly()), "ResGui", key, language);
			}
			catch
			{
				result = "No localized GuiMessage founded for key '" + key + "'";
			}
			return result;
		}
		public static string GetHelpString(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key == string.Empty)
			{
				throw new ArgumentException("key is empty", "key");
			}
			string result;
			try
			{
				result = (string)LocalizeDictionary.Instance.GetLocalizedObject<object>(LocalizeDictionary.Instance.GetAssemblyName(Assembly.GetExecutingAssembly()), "ResHelp", key, LocalizeDictionary.Instance.Culture);
			}
			catch
			{
				result = "No localized HelpMessage founded for key '" + key + "'";
			}
			return result;
		}
		public static string GetMaintenanceString(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key == string.Empty)
			{
				throw new ArgumentException("key is empty", "key");
			}
			string result;
			try
			{
				result = (string)LocalizeDictionary.Instance.GetLocalizedObject<object>(LocalizeDictionary.Instance.GetAssemblyName(Assembly.GetExecutingAssembly()), "ResMaintenance", key, LocalizeDictionary.Instance.Culture);
			}
			catch
			{
				result = "No localized MaintenanceMessage founded for key '" + key + "'";
			}
			return result;
		}
		public static string GetUpdateAgentString(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key == string.Empty)
			{
				throw new ArgumentException("key is empty", "key");
			}
			string result;
			try
			{
				result = (string)LocalizeDictionary.Instance.GetLocalizedObject<object>(LocalizeDictionary.Instance.GetAssemblyName(Assembly.GetExecutingAssembly()), "ResUpdateAgent", key, LocalizeDictionary.Instance.Culture);
			}
			catch
			{
				result = "No localized UpdateAgentMessage founded for key '" + key + "'";
			}
			return result;
		}
	}
}
