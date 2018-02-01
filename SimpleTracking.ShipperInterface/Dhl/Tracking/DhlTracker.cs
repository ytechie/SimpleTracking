using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
{
	public class DhlTracker : ITracker
	{
		private const string DEV_URL = "HTTPS://eCommerce.Airborne.com/APILandingTest.asp";
		private const string PRODUCTION_URL = "HTTPS://eCommerce.Airborne.com/APILanding.asp";

		public const string USAGE_REQUIREMENTS =
			"NOTICE: DHL tracking data is the sole private property of DHL and may only be used to track shipments tendered by, to or for you to DHL for delivery and for no other purpose." +
            " Any other use of HDL tracking data is strictly prohibited.";

		private readonly string _password;
		private readonly IWebPoster _postUtility;
		private readonly string _username;

		public DhlTracker(IWebPoster postUtility, string username, string password)
		{
			_postUtility = postUtility;
			_username = username;
			_password = password;
		}

		#region ITracker Members

		public TrackingData GetTrackingData(string trackingNumber)
		{
			string requestXml = TrackingRequest.GetTrackingRequest(trackingNumber, _username, _password);
			string responseXml = _postUtility.PostData(DEV_URL, requestXml);

			return TrackingResponse.GetCommonTrackingData(responseXml);
		}

		#endregion

		public static bool IsValidTrackingNumber(string trackingNumber)
		{
			//The best we can do is check the length because DHL doesn't appear to have a check digit
			if (trackingNumber.Length != 11 && trackingNumber.Length != 10)
				return false;

			return true;
		}
	}
}