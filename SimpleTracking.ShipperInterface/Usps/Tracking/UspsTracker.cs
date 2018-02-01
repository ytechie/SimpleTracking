using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Common;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;

namespace SimpleTracking.ShipperInterface.Usps.Tracking
{
	/// <summary>
	///		A class for tracking USPS packages.
	/// </summary>
	public class UspsTracker : ITracker
	{
		/// <summary>
		///		The USPS development API URL
		/// </summary>
		public const string DEV_URL = "http://testing.shippingapis.com/ShippingAPITest.dll?API=TrackV2&XML={0}";

		/// <summary>
		///		The USPS production API URL
		/// </summary>
		public const string PROD_URL = "http://production.shippingapis.com/ShippingAPI.dll?API=TrackV2&XML={0}";

		private readonly string _password;
		private readonly IWebPoster _postUtility;
		private readonly string _serviceUrl;
		private readonly string _userName;

		/// <summary>
		///		Creates a new instance of the <see cref="UspsTracker"/> class.
		/// </summary>
		/// <param name="postUtility">
		///		The <see cref="IWebPoster"/> to use to send and receive data
		///		from the API service.
		/// </param>
		/// <param name="userName">
		///		A valid USPS API user name.
		/// </param>
		/// <param name="password">
		///		A valid USPS API password.
		/// </param>
		/// <param name="production">
		///		A boolean indicating if this tracker is being used in development or production.
		/// </param>
		public UspsTracker(IWebPoster postUtility, string userName, string password, bool production)
		{
			_postUtility = postUtility;
			_userName = userName;
			_password = password;

			if (production)
				_serviceUrl = PROD_URL;
			else
				_serviceUrl = DEV_URL;
		}

		#region ITracker Members

		/// <summary>
		///		Gets the <see cref="TrackingData"/> for the specified tracking number
		///		by calling the API and parsing the results.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to request tracking information for.
		/// </param>
		/// <returns>
		///		A <see cref="TrackingData"/> instance that represents the information
		///		returned from the API.
		/// </returns>
		public TrackingData GetTrackingData(string trackingNumber)
		{
			if(!IsUspsTrackingNumber(trackingNumber))
				return null;

			string requestXml = TrackingRequest.GetTrackingRequest(trackingNumber, _userName, _password);
			string requestUrl = string.Format(_serviceUrl, requestXml);
			string responseXml = _postUtility.PostData(requestUrl, null);
			TrackingData td = TrackingResponse.GetCommonTrackingData(responseXml);

			return td;
		}

		#endregion

		/// <summary>
		///		Determines if the specified tracking number is a USPS
		///		tracking number by using a standard MOD 10 algorithm
		///		to check the check digit.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to check the check digit of.
		/// </param>
		/// <returns>
		///		True if the tracking number is the correct length and contains
		///		a valid check digit, otherwise false.
		/// </returns>
		public static bool IsUspsTrackingNumber(string trackingNumber)
		{
			if (trackingNumber.Length != 22 && trackingNumber.Length != 20)
				return false;

			return CheckDigits.CheckMod10CheckDigit(trackingNumber);
		}
	}
}