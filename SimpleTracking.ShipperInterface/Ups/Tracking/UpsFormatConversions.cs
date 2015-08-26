using System;
using System.Globalization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking
{
	public class UpsFormatConversions
	{
		public const string UPS_DATE_FORMAT = "yyyyMMdd";
		public const string UPS_TIME_FORMAT = "HHmmss";

		private UpsFormatConversions()
		{
		}

		public static DateTime parseUpsDate(string upsDateString)
		{
			return DateTime.ParseExact(upsDateString, UPS_DATE_FORMAT, CultureInfo.CurrentCulture);
		}

		public static DateTime parseUpsTime(string upsTimeString)
		{
			return DateTime.ParseExact(upsTimeString, UPS_TIME_FORMAT, CultureInfo.CurrentCulture);
		}

		public static DateTime parseUpsDateTime(string datePart, string timePart)
		{
			//Sample date format: 20041124
			//Sample time format: 161700

			return DateTime.ParseExact(datePart + " " + timePart, UPS_DATE_FORMAT + " " + UPS_TIME_FORMAT, CultureInfo.CurrentCulture);
		}
	}
}
