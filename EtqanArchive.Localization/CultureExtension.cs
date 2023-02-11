using System;
using System.Globalization;

namespace EtqanArchive.Localization
{
	public static class CultureExtension
	{
		public static bool IsArabic(this CultureInfo CultureInfo)
		{
			if (CultureInfo.Name == "ar" || CultureInfo.Name == "ar-EG")
			{
				return true;
			}
			return false;
		}

		public static string ToDBDate(this DateTime datetime)
		{
			DateTimeFormatInfo usDtfi = new CultureInfo("en-US", false).DateTimeFormat;
			return datetime.ToString("dd-MMM-yyyy", usDtfi);

		}
		public static string ToDefualtDateFormat(this DateTime datetime)
		{
			DateTimeFormatInfo usDtfi = new CultureInfo("en-US", false).DateTimeFormat;
			return datetime.ToString("dd/MM/yyyy", usDtfi);

		}
	}
}
