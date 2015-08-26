using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Geocoding;

namespace SimpleTracking.ShipperInterface.Tracking
{
    public class GeocodingTracker : ITracker
    {
        private readonly ITracker _baseTracker;
        private readonly IGeocodeDb _geocoder;

        public GeocodingTracker(ITracker baseTracker, IGeocodeDb geocoder)
        {
            _baseTracker = baseTracker;
            _geocoder = geocoder;
        }

        public TrackingData GetTrackingData(string trackingNumber)
        {
            var trackingData = _baseTracker.GetTrackingData(trackingNumber);

            if (trackingData.Activity != null && trackingData.Activity.Count > 0)
            {
                foreach (var activity in trackingData.Activity)
                {
                    if (!string.IsNullOrWhiteSpace(activity.LocationDescription))
                    {
                        var address = LocationParser.ParseLocation(activity.LocationDescription);
                        var cityDetails = Geocode(address);
                        if (cityDetails != null)
                        {
                            activity.Latitude = cityDetails.Latitude;
                            activity.Longitude = cityDetails.Longitude;
                        }
                    }
                }
            }

            return trackingData;
        }

        private CityRecord Geocode(AddressLocation location)
        {
            if (!string.IsNullOrWhiteSpace(location.City) && !string.IsNullOrWhiteSpace(location.State))
            {
                return _geocoder.GetCity(location.City, location.State);
            }
            else
            {
                return null;
            }
        }
    }
}
