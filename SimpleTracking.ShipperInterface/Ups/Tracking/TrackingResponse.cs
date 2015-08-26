using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml;
using log4net;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Common;
using SimpleTracking.ShipperInterface.Tracking;

namespace SimpleTracking.ShipperInterface.Ups.Tracking
{
    /// <summary>
    ///		A class for processing the XML tracking response from UPS.
    /// </summary>
    public class TrackingResponse
    {
        /// <summary>
        ///		Declare and create our logger
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///		The friendly name of the shipper to include in the tracking data.
        /// </summary>
        public const string SHIPPER_NAME = "UPS";

        /// <summary>
        ///		Specific usage requirements that should be displayed to the user.
        /// </summary>
        public const string USAGE_REQUIREMENTS = "NOTICE: The UPS package tracking systems accessed via this service (the \"Tracking Systems\") and tracking information" +
            " obtained through this service (the \"Information\") are the private property of UPS. UPS authorizes you to use the Tracking Systems solely to track shipments" +
            " tendered by or for you to UPS for delivery and for no other purpose. Without limitation, you are not authorized to make the Information available on any web" +
            " site or otherwise reproduce, distribute, copy, store, use or sell the Information for commercial gain without the express written consent of UPS. This is a" +
            " personal service, thus your right to use the Tracking Systems or Information is non-assignable. Any access or use that is inconsistent with these terms is" +
            " unauthorized and strictly prohibited.";

        /// <summary>
        ///		Converts a "Scan" node in the XML document into an <see cref="Activity"/>.
        /// </summary>
        /// <param name="scanNode">
        ///		The <see cref="XmlNode"/> that represents a "scan" node.
        /// </param>
        /// <returns>
        ///		An activity that contains the data from the scan.
        /// </returns>
        private static Activity getActivityFromScanNode(XmlNode scanNode)
        {
            var activity = new Activity();

            var dateString = scanNode.SelectSingleNode("Date/text()").Value;
            var timeString = scanNode.SelectSingleNode("Time/text()").Value;

            var date = UpsFormatConversions.parseUpsDate(dateString);
            var time = UpsFormatConversions.parseUpsTime(timeString);

            activity.Timestamp = date.Add(time.TimeOfDay);

            var scanDescriptionNode = scanNode.SelectSingleNode("Status/StatusType/Description/text()");
            if (scanDescriptionNode != null)
                activity.ShortDescription = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(scanDescriptionNode.Value.ToLower());

            string city = null, state = null, countryCode = null;

            var node = scanNode.SelectSingleNode("ActivityLocation/Address/City/text()");
            if (node != null)
            {
                city = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(node.Value.ToLower());
            }
            node = scanNode.SelectSingleNode("ActivityLocation/Address/StateProvinceCode/text()");
            if (node != null)
                state = node.Value;
            node = scanNode.SelectSingleNode("ActivityLocation/Address/CountryCode/text()");
            if (node != null)
                countryCode = node.Value;

            activity.LocationDescription = Location.GetLocationString(city, state, countryCode);

            return activity;
        }

        /// <summary>
        ///		
        /// </summary>
        /// <param name="xmlDoc">
        ///		The XML document to read the activities from.
        /// </param>
        /// <returns>
        ///		A list of <see cref="Activity"/> objects that represent
        ///		the scans.
        /// </returns>
        private static List<Activity> getActivities(XmlNode xmlDoc)
        {
            var activities = new List<Activity>();

            var scanNodes = xmlDoc.SelectNodes("/TrackResponse/Shipment/Package/Activity");
            if (scanNodes != null)
                foreach (XmlNode currScanNode in scanNodes)
                    activities.Add(getActivityFromScanNode(currScanNode));

            activities.Reverse();

            foreach (var activity in activities)
                activity.Stage = GetActivityType(activity.ShortDescription);

            return activities;
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
            _log.Debug("Processing UPS tracking response data: " + xml);

            try
            {
                var td = new TrackingData();

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                //Verify that the shipment information is available
                //in the XML, if it isn't, they must not have any
                //tracking data yet.
                var node = xmlDoc.SelectSingleNode("/TrackResponse/Shipment");
                if (node == null)
                    return null;

                node = xmlDoc.SelectSingleNode("/TrackResponse/Shipment/ScheduledDeliveryDate/text()");
                if (node != null)
                    td.EstimatedDelivery = UpsFormatConversions.parseUpsDate(node.Value);

                var nodes = xmlDoc.SelectNodes("/TrackResponse/Shipment/ReferenceNumber/Value/text()");
                for (var i = 0; i < nodes.Count; i++)
                    td.ReferenceNumbers.Add(nodes[i].Value);

                node = xmlDoc.SelectSingleNode("/TrackResponse/Shipment/Service/Description/text()");
                if(node != null)
                    td.ServiceType = node.Value;

                node = xmlDoc.SelectSingleNode("/TrackResponse/Shipment/Package/PackageWeight/Weight/text()");
                if(node != null)
                    td.Weight = decimal.Parse(node.Value);

                td.Activity = getActivities(xmlDoc);

                td.TrackerName = SHIPPER_NAME;
                td.UsageRequirements = USAGE_REQUIREMENTS;

                td.LastUpdated = DateTime.Now;
                
                return td;
            }
            catch (Exception ex)
            {
                throw new ResponseParseException(xml, ex);
            }
        }

        /// <summary>
        ///		Determines if the string is a valid UPS tracking number
        ///		according to the UPS checksum algorithms.
        /// </summary>
        /// <param name="trackingNumber">
        ///		The tracking number to classify as valid or invalid.
        /// </param>
        /// <returns>
        ///		A boolean indicating if the tracking number passes the
        ///		check digit calculations.
        /// </returns>
        public static bool IsValidTrackingNumber(string trackingNumber)
        {
            //Handle 1Z tracking numbers a special way
            if (trackingNumber.StartsWith("1z", true, CultureInfo.InvariantCulture))
            {
                //Trim off the 1Z
                trackingNumber = trackingNumber.Substring(2);

                //Encode the letters into numbers based on the cross-reference
                trackingNumber = ConvertToNumeric(trackingNumber);

                return Check1ZMod10CheckDigit(trackingNumber);
            }

            if (trackingNumber.Length != 11)
                return false;

            //Encode the letters into numbers based on the cross-reference
            trackingNumber = ConvertToNumeric(trackingNumber);

            return CheckDigits.CheckMod10CheckDigit(trackingNumber);
        }

        public static string ConvertToNumeric(string mixedTrackingNumber)
        {
            var chars = mixedTrackingNumber.ToCharArray();

            for (var i = 0; i < chars.Length; i++)
            {
                if (char.IsLetter(chars[i]))
                    chars[i] = GetTrackingNumberCrossReferenceValue(chars[i]).ToString()[0];
            }

            return new string(chars);
        }

        public static int GetTrackingNumberCrossReferenceValue(char inChar)
        {
            //Make sure the char is upper case
            inChar = inChar.ToString().ToUpper()[0];

            var charValue = (int)inChar;

            return (charValue - 63) % 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///		From UPS:
        ///			From left, add all odd positions.
        ///			From left, add all even positions and multiply by two.
        ///			Add results of steps 3 and 4.
        ///			Subtract result from next highest multiple of 10.
        ///			The remainder is your check digit [the last digit].   
        /// </remarks>
        /// <param name="checkString"></param>
        /// <returns></returns>
        public static bool Check1ZMod10CheckDigit(string checkString)
        {
            var chars = checkString.ToCharArray();
            var num1 = 0;
            for (var i = 0; i < chars.Length; i += 2)
                num1 += (chars[i] - 48);

            var num2 = 0;
            for (var i = 1; i < chars.Length - 1; i += 2)
                num2 += (chars[i] - 48);

            num2 *= 2;

            num1 += num2;

            var checkDigit = 10 - (num1 % 10);
            if (checkDigit == 10)
                checkDigit = 0;

            return checkString.EndsWith(checkDigit.ToString());
        }

        /// <summary>
        ///     Converts a UPS activity description into our activity enum.
        /// </summary>
        /// <param name="activityDescription"></param>
        public static ShipmentStage? GetActivityType(string activityDescription)
        {
            if (string.Compare(activityDescription, "Pickup Manifest Received", StringComparison.CurrentCultureIgnoreCase) == 0)
                return ShipmentStage.Created;
            if (string.Compare(activityDescription, "Pickup Scan", StringComparison.CurrentCultureIgnoreCase) == 0)
                return ShipmentStage.Scan;
            if (string.Compare(activityDescription, "Location Scan", StringComparison.CurrentCultureIgnoreCase) == 0)
                return ShipmentStage.Scan;
            if (string.Compare(activityDescription, "Departure Scan", StringComparison.CurrentCultureIgnoreCase) == 0)
                return ShipmentStage.Scan;
            if (string.Compare(activityDescription, "Arrival Scan", StringComparison.CurrentCultureIgnoreCase) == 0)
                return ShipmentStage.Scan;
            if (string.Compare(activityDescription, "Destination Scan", StringComparison.CurrentCultureIgnoreCase) == 0)
                return ShipmentStage.Scan;
            if (string.Compare(activityDescription, "Delivered", StringComparison.CurrentCultureIgnoreCase) == 0)
                return ShipmentStage.Delivered;

            _log.WarnFormat("Unknown activity description: {0}", activityDescription);

            return null;
        }
    }
}
