using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using log4net;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Common;
using SimpleTracking.ShipperInterface.Tracking;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
{
	/// <summary>
	///		Methods for working with the XML response from
	///		the DHL API web service.
	/// </summary>
	public static class TrackingResponse
	{
		/// <summary>
		///		Declare and create our logger
		/// </summary>
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		///		The unique tracker name for this particular tracker.
		/// </summary>
		public const string TRACKER_NAME = "DHL";

		private static List<Activity> getActivities(XmlNode xmlDoc)
		{
			const string XPATH_STEP = "//Shipment/TrackingHistory/Status";

			var activities = new List<Activity>();

			XmlNodeList scanNodes = xmlDoc.SelectNodes(XPATH_STEP);
			foreach (XmlNode currScanNode in scanNodes)
				activities.Add(getActivityFromScanNode(currScanNode));

			Activity pickupActivity = getPickupActivity(xmlDoc);
			if (pickupActivity != null)
				activities.Add(pickupActivity);

			activities.Reverse();

			return activities;
		}

		private static Activity getPickupActivity(XmlNode xmlDoc)
		{
			XmlNode node = xmlDoc.SelectSingleNode("//Pickup/Date/text()");
			if (node == null)
				return null;
			string dateString = node.Value;
			node = xmlDoc.SelectSingleNode("//Pickup/Time/text()");
			if(node != null)
				dateString += " " + node.Value;

			var activity = new Activity();
			activity.Timestamp = DateTime.Parse(dateString);
			activity.ShortDescription = "Picked up by DHL.";

			return activity;
		}

		private static Activity getActivityFromScanNode(XmlNode scanNode)
		{
			var activity = new Activity();
			XmlNode currNode;

			var dateString = scanNode.SelectSingleNode("Date/text()").Value;
			dateString += " " + scanNode.SelectSingleNode("Time/text()").Value;

			activity.Timestamp = DateTime.Parse(dateString);
			activity.ShortDescription = scanNode.SelectSingleNode("StatusDesc/text()").Value;

			string city = null, state = null, country = null;

			currNode = scanNode.SelectSingleNode("Location/City/text()");
			if(currNode != null)
				city = currNode.Value;
			currNode = scanNode.SelectSingleNode("Location/State/text()");
			if (currNode != null)
				state = currNode.Value;
			currNode = scanNode.SelectSingleNode("Location/Country/text()");
			if (currNode != null)
				country = currNode.Value;
			var location = Location.GetLocationString(city, state, country);

			activity.LocationDescription = location;

			return activity;
		}

		public static KeyValuePair<int, string> GetResponseCode(XmlDocument xml)
		{
			const string XPATH = @"//Shipment/Result";

			var resultNode = xml.SelectSingleNode(XPATH);

			var resultCode = int.Parse(resultNode.ChildNodes[0].InnerText);
			var resultDesc = resultNode.ChildNodes[1].InnerText;
			var response = new KeyValuePair<int, string>(resultCode, resultDesc);

			return response;
		}

		/// <summary>
		///		Puts the tracking response data into the common
		///		<see cref="TrackingData"/> format.
		/// </summary>
		/// <returns>
		///		Gets the <see cref="TrackingData"/> that contains the tracking
		///		information for this response.
		/// </returns>
		public static TrackingData GetCommonTrackingData(string xml)
		{
			_log.Debug("Processing DHL tracking response data: " + xml);

			try
			{
				var td = new TrackingData();

				var xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xml);

				var response = GetResponseCode(xmlDoc);
				//Response code 0 = Package found
				if (response.Key != 0)
				{
					//Response code 1 = Not found, anything else should log the message
					if(response.Key != 1)
						_log.InfoFormat("Received a response code of {0} from DHL, message: {1}", response.Key, response.Value);
					return null;
				}

				td.Activity = getActivities(xmlDoc);

				td.TrackerName = TRACKER_NAME;
				td.UsageRequirements = DhlTracker.USAGE_REQUIREMENTS;

				return td;
			}
			catch (Exception ex)
			{
				throw new ResponseParseException(xml, ex);
			}
		}
	}
}