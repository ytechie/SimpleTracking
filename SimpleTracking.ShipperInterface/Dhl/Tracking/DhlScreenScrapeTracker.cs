using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
{
	/// <summary>
	///		A tracker for DHL shipments that reads the data from
	///		the DHL web tracking page.
	/// </summary>
	/// <remarks>
	///		The data is actually in hidden form fields in XML form, so
	///		while it's technically screen scraping, we have less worries
	///		about the data changing on us.
	/// </remarks>
	public class DhlScreenScrapeTracker : ITracker
	{
		private const string POST_URL = "http://track.dhl-usa.com/TrackByNbr.asp?nav=Tracknbr";

		private readonly IWebPoster _postUtility;

		/// <summary>
		///		Creates a new instance of the <see cref="DhlScreenScrapeTracker"/>
		/// </summary>
		/// <param name="postUtility">
		///		The <see cref="IWebPoster"/> to use to make the web requests.
		/// </param>
		public DhlScreenScrapeTracker(IWebPoster postUtility)
		{
			_postUtility = postUtility;
		}

		#region ITracker Members

		/// <summary>
		///		Gets the DHL tracking details for a tracking number.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to retrieve tracking details for.
		/// </param>
		/// <returns>
		///		The tracking data extracted from the tracking response.
		/// </returns>
		public TrackingData GetTrackingData(string trackingNumber)
		{
			if (!DhlTracker.IsValidTrackingNumber(trackingNumber))
				return null;

			string requestString = string.Format("txtTrackNbrs={0}", trackingNumber);
			string responseXml = _postUtility.PostData(POST_URL, requestString);

			return ScreenScrapeResponse.GetCommonTrackingData(responseXml);
		}

		#endregion
	}
}