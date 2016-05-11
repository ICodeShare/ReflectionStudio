using Localization.Properties;
using System;
using System.Linq;
namespace Localization.Extensions
{
	public static class StringLocalizationExtensions
	{
		public static string AddBlanks(this string value)
		{
			return string.Join("", value.Select(delegate(char x)
			{
				if (!char.IsUpper(x))
				{
					return x.ToString();
				}
				return " " + x;
			})).Trim();
		}
		public static string AsLocalized(this string value)
		{
			if (value.Contains(" "))
			{
				return value;
			}
			string @string = Resources.ResourceManager.GetString(value);
			if (string.IsNullOrEmpty(@string))
			{
				return value.AddBlanks();
			}
			return @string;
		}
	}
}
