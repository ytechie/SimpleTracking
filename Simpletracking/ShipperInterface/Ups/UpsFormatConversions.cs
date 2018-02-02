using System;
using System.Globalization;

namespace SimpleTracking.ShipperInterface.Ups
{
	/// <summary>
	///		Methods for working with pieces of data in the UPS request/reponse XML.
	/// </summary>
	public static class UpsFormatConversions
	{
		/// <summary>
		///		The date format in the UPS XML requests/responses.
		/// </summary>
		public const string UPS_DATE_FORMAT = "yyyyMMdd";
		/// <summary>
		///		The time format in the UPS XML requests/responses.
		/// </summary>
		public const string UPS_TIME_FORMAT = "HHmmss";

		/// <summary>
		///		Parses a UPS formatted date string into a <see cref="DateTime"/> object.
		/// </summary>
		/// <param name="upsDateString"></param>
		/// <returns></returns>
		public static DateTime ParseUpsDate(string upsDateString)
		{
			return DateTime.ParseExact(upsDateString, UPS_DATE_FORMAT, CultureInfo.CurrentCulture);
		}

		/// <summary>
		///		Parses a UPS formatted time string into a <see cref="DateTime"/> object.
		/// </summary>
		/// <param name="upsTimeString"></param>
		/// <returns></returns>
		public static DateTime ParseUpsTime(string upsTimeString)
		{
			return DateTime.ParseExact(upsTimeString, UPS_TIME_FORMAT, CultureInfo.CurrentCulture);
		}

		/// <summary>
		///		Parses a UPS formatted date string and time string into a <see cref="DateTime"/> object.
		/// </summary>
		/// <param name="datePart"></param>
		/// <param name="timePart"></param>
		/// <returns></returns>
		public static DateTime parseUpsDateTime(string datePart, string timePart)
		{
			return
				DateTime.ParseExact(datePart + " " + timePart, UPS_DATE_FORMAT + " " + UPS_TIME_FORMAT, CultureInfo.CurrentCulture);
		}
	}
}