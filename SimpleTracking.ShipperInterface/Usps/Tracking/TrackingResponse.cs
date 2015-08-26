using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using log4net;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;

namespace SimpleTracking.ShipperInterface.Usps.Tracking
{
	/// <summary>
	///		Methods to work with handling the XML response from the
	///		USPS API.
	/// </summary>
	public static class TrackingResponse
	{
		/// <summary>
		///		Declare and create our logger
		/// </summary>
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string UsageRequirements = "NOTICE: U.S.P.S. authorizes you to use U.S.P.S. tracking systems solely to track shipments tendered by or for you" +
            " to U.S.P.S. for delivery and for no other purpose. Any other use of U.S.P.S. tracking systems and information is strictly prohibited.";

		/// <summary>
		///		Converts the USPS XML response data into a <see cref="TrackingData"/> instance.
		/// </summary>
		/// <param name="xml">
		///		The XML response from the API call.
		/// </param>
		/// <returns>
		///		The XML parsed into a <see cref="TrackingData"/> instance.
		/// </returns>
		public static TrackingData GetCommonTrackingData(string xml)
		{
			_log.Debug("Processing USPS tracking response data: " + xml);

			try
			{
				var td = new TrackingData();
				td.Activity = GetActivities(xml);
                td.UsageRequirements = UsageRequirements;

				return td;
			}
			catch (Exception ex)
			{
				throw new ResponseParseException(xml, ex);
			}
		}

		/// <summary>
		///		Gets a list of activities from the USPS API response XML.
		/// </summary>
		/// <param name="xml">
		///		The XML response from the API call.
		/// </param>
		/// <returns>
		///		The API XML parsed into a list of <see cref="Activity"/> objects.
		/// </returns>
		public static List<Activity> GetActivities(string xml)
		{
			const string STEP_XPATH = "/TrackResponse/TrackInfo/TrackSummary/text()|/TrackResponse/TrackInfo/TrackDetail/text()";

			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);

			var activities = new List<Activity>();
			XmlNodeList nodes = xmlDoc.SelectNodes(STEP_XPATH);
			foreach (XmlNode currNode in nodes)
			{
				Activity activity = ParseActivityString(currNode.Value);
				activities.Add(activity);
			}

			//The xml document contains the activities in reverse order
			activities.Reverse();

			return activities;
		}

		/// <summary>
		///		Parses an individual activity node value into an <see cref="Activity"/>
		/// </summary>
		/// <param name="stepDetails">
		///		The text from a TrackSummary or TrackDetail XML node.
		/// </param>
		/// <returns>
		///		An <see cref="Activity"/> that represents the data from the step.
		/// </returns>
		public static Activity ParseActivityString(string stepDetails)
		{
			const string DETAIL_REGEX =
				@"(?<description>Electronic Shipping Info Received), (?<date>.*)|(?<description>Processed), (?<date>.*, \d{4}), (?<time>.*?), (?<location>.*)|(?<description>Your item was delivered) at (?<time>.*) on (?<date>.*?) in (?<location>[^.]*)\.* *(?<extraInfo>.*)";
			
			var activity = new Activity();

			Match match = Regex.Match(stepDetails, DETAIL_REGEX);

			Group group = match.Groups["description"];
			if (!group.Success)
			{
				activity.ShortDescription = stepDetails;
				return activity;
			}

			activity.ShortDescription = group.Value;

			group = match.Groups["date"];
			if (group.Success)
			{
				string dateString = group.Value;

				group = match.Groups["time"];
				if (group.Success)
					dateString += " " + group.Value;

				activity.Timestamp = DateTime.Parse(dateString);
			}

			group = match.Groups["location"];
			if (group.Success)
				activity.LocationDescription = group.Value;

			return activity;
		}
	}
}