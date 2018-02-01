using System.Text;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking.Rss
{
	/// <summary>
	///		Implements <see cref="IRssBodyFormatter"/> to format the body
	///		of an RSS feed item for a package that's being tracked.
	/// </summary>
	public class SimpleTrackingRssBodyFormatter : IRssBodyFormatter
	{
		#region IRssBodyFormatter Members

		/// <summary>
		///		Gets a formatted body for an RSS item for use on SimpleTracking.com.
		/// </summary>
		/// <param name="activity">
		///		The activity that this feed body is being created for.
		/// </param>
		/// <returns>
		///		A string to use for the body of the RSS feed item.
		/// </returns>
		public string GetFormattedBody(Activity activity)
		{
			var sb = new StringBuilder();

			sb.AppendFormat("Date/Time: {0}<br />", activity.Timestamp);
			sb.AppendFormat("Location: {0}", activity.LocationDescription);

			sb.Append("<hr />");
			sb.Append("<a href=\"http://www.SimpleTracking.com?source=feed-footer-click\">");
			sb.Append(
				"<img src=\"http://www.SimpleTracking.com/Images/Rss-Footer-Image.gif\" alt=\"Powered by SimpleTracking.com\" />");
			sb.Append("</a>");

			return sb.ToString();
		}

		#endregion
	}
}