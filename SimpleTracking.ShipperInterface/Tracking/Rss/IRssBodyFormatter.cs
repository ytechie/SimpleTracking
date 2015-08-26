using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking.Rss
{
	/// <summary>
	///		Represents a class that is able to get a formatted body string
	///		for an item in an RSS feed.
	/// </summary>
	public interface IRssBodyFormatter
	{
		/// <summary>
		///		Gets the RSS item HTML for the specified activity.
		/// </summary>
		/// <param name="activity">
		///		The activity to generate the RSS item HTML for.
		/// </param>
		/// <returns>
		///		HTML that can be used in an RSS item to represent this
		///		activity.
		/// </returns>
		string GetFormattedBody(Activity activity);
	}
}