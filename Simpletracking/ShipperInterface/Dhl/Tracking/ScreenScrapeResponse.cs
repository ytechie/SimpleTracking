using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
{
	/// <summary>
	///		Handles the web response from the DHL web tracking service.
	/// </summary>
	public class ScreenScrapeResponse
	{
		private const string REGEX_XML_ELEMENTS =
			"<input type=\"hidden\" name=\"hdnXMLResponse_[\\d]+\" value=\"([\\S\\s]*?)\"/>";

		public static string GetDhlTrackingXml(string html)
		{
			MatchCollection matches = Regex.Matches(html, REGEX_XML_ELEMENTS);
			var sb = new StringBuilder();
			foreach (Match currMatch in matches)
			{
				sb.Append(currMatch.Groups[1].Value);
			}

			return HttpUtility.HtmlDecode(sb.ToString());
			
		}

		/// <summary>
		///		Parses the HTML response from DHL and extracts the tracking data.
		/// </summary>
		/// <param name="html">
		///		The HTML response from the DHL track request web site.
		/// </param>
		/// <returns>
		///		The parsed tracking data.
		/// </returns>
		public static TrackingData GetCommonTrackingData(string html)
		{
			if (html.Contains("The following Tracking Number(s) are not valid"))
			{
				return new TrackingData
				       	{
				       		TrackerName = TrackingResponse.TRACKER_NAME,
				       		UsageRequirements = DhlTracker.USAGE_REQUIREMENTS,
				       		Activity = new List<Activity>()
				       	};
			}

			return TrackingResponse.GetCommonTrackingData(GetDhlTrackingXml(html));
		}
	}
}