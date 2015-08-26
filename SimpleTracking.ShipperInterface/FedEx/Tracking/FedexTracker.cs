using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Common;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;
using System;

namespace SimpleTracking.ShipperInterface.FedEx.Tracking
{
	/// <summary>
	///		The <see cref="ITracker"/> class for tracking FedEx packages.
	/// </summary>
	public class FedexTracker : ITracker
	{
        private const string DEV_URL = "https://wsbeta.fedex.com/web-services";
        private const string PRODUCTION_URL = "https://ws.fedex.com/web-services";

        private const string UsageRequirements = "NOTICE: FedEx authorizes you to use FedEx tracking systems solely to track shipments tendered by" +
            " or for you to FedEx for delivery and for no other purpose. Any other use of FedEx tracking systems and information is strictly prohibited.";

        private readonly string _key;
        private readonly string _password;
		private readonly string _accountNumber;
		private readonly string _meterNumber;
		private readonly ITrackService _trackService;

		public FedexTracker(ITrackService trackService, string key, string password, string accountNumber, string meterNumber, bool production)
		{
            _trackService = trackService;
            _key = key;
            _password = password;
			_accountNumber = accountNumber;
			_meterNumber = meterNumber;

			if (production)
				_trackService.Url = PRODUCTION_URL;
			else
                _trackService.Url = DEV_URL;
		}

		#region ITracker Members

		/// <summary>
		///		Gets the tracking data for the specified tracking number.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to get tracking data for.
		/// </param>
		/// <returns>
		/// </returns>
		public TrackingData GetTrackingData(string trackingNumber)
		{
			//Only track it if it's a valid FedEx express or ground number
			if (!IsFedExExpress(trackingNumber) && !IsFedExGround(trackingNumber) && !IsFedExSmartPost(trackingNumber))
				return null;

            var trackRequest = CreateTrackRequest(_key, _password, _accountNumber, _meterNumber, trackingNumber);

            //Note: If you get an exception creating the web service, open Reference.cs, and replace all [][]'s with []
            //Reference: http://brianseekford.com/index.php/2010/05/06/fedex-integration-address-service-unable-to-generate-a-temporary-class-parsedelement/
            //Reference: http://support.microsoft.com/kb/2486643
            
            var resp = _trackService.track(trackRequest);

            var td = new TrackingData
            {
                TrackerName = "FedEx",
            };

            //No activity, no data
            if(resp == null || resp.TrackDetails == null || resp.TrackDetails.Length == 0)
                return td;

            td.EstimatedDelivery = resp.TrackDetails[0].EstimatedDeliveryTimestamp;

            foreach (var evt in resp.TrackDetails[0].Events)
            {
                var a = new Activity();
                a.LocationDescription = evt.Address.GetFriendlyAddressString();
                a.ShortDescription = evt.EventDescription;
                a.Timestamp = evt.Timestamp;

                td.Activity.Add(a);
            }

            td.UsageRequirements = UsageRequirements;

            return td;
		}

        private static FedexTrackWebService.TrackRequest CreateTrackRequest(string key, string password, string accountNumber, string meterNumber, string trackingNumber)
        {
            FedexTrackWebService.TrackRequest request = new FedexTrackWebService.TrackRequest
            {
                // Date range is optional.
                // If omitted, set to false
                ShipDateRangeBeginSpecified = false,
                ShipDateRangeEndSpecified = false,

                // Include detailed scans is optional.
                // If omitted, set to false
                IncludeDetailedScans = true,
                IncludeDetailedScansSpecified = true
            };

            request.WebAuthenticationDetail = new FedexTrackWebService.WebAuthenticationDetail
            {
                UserCredential = new FedexTrackWebService.WebAuthenticationCredential
                {
                    Key = key,
                    Password = password
                }
            };

            request.ClientDetail = new FedexTrackWebService.ClientDetail
            {
                AccountNumber = accountNumber,
                MeterNumber = meterNumber
            };

            request.TransactionDetail = new FedexTrackWebService.TransactionDetail();
            request.Version = new FedexTrackWebService.VersionId();

            request.PackageIdentifier = new FedexTrackWebService.TrackPackageIdentifier
            {
                Value = trackingNumber,
                Type = FedexTrackWebService.TrackIdentifierType.TRACKING_NUMBER_OR_DOORTAG
            };

            return request;
        }

		#endregion

		/// <summary>
		///		Runs the FedEx checksum algorithm and length check on the tracking number.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to verify the check digit on.
		/// </param>
		/// <returns>
		///		True if the tracking number appears to be a valid FedEx express tracking number,
		///		otherwise false is returned.
		/// </returns>
		public static bool IsFedExExpress(string trackingNumber)
		{
			if (trackingNumber.Length != 12)
				return false;

			char[] chars = trackingNumber.ToCharArray();
			int digitSum = 0;
			for (int i = 0; i < chars.Length - 1; i++)
			{
				//The number can only contain digits
				if (!char.IsDigit(chars[i]))
					return false;

				if (i%3 == 0)
					digitSum += (chars[i] - 48)*3;
				else if ((i - 2)%3 == 0)
					digitSum += (chars[i] - 48)*7;
				else
					digitSum += (chars[i] - 48);
			}

			int checkDigit = digitSum%11;
			if (checkDigit == 10)
				checkDigit = 0;

			//Verify that the last digit matches the expected check digit
			return (chars[11] - 48) == checkDigit;
		}

		/// <summary>
		///		Runs the FedEx checksum algorithm and length check on the ground tracking number.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to verify the check digit on.
		/// </param>
		/// <returns>
		///		True if the tracking number appears to be a valid FedEx ground tracking number,
		///		otherwise false is returned.
		/// </returns>
		public static bool IsFedExGround(string trackingNumber)
		{
			if (trackingNumber.Length != 15)
				return false;

			return CheckDigits.CheckMod10CheckDigit(trackingNumber);
		}

        public static bool IsFedExSmartPost(string trackingNumber)
        {
            //The best we can do now is to check the length. Ideally we'll figure out the algorithm
            //so that we can properly check it.

            if (trackingNumber.Length != 22)
                return false;

            return true;
        }
	}
}