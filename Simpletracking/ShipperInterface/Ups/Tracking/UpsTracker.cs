using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Tracking;

namespace SimpleTracking.ShipperInterface.Ups.Tracking
{
    public class UpsTracker : ITracker
    {
        public TrackingData GetTrackingData(string trackingNumber)
        {
            var ar = new AccessRequest("ACBB0DA11CE56A06", "ytechie", "sVUbis6Kla");
            var tr = new TrackingRequest(trackingNumber, "req");
            var td = tr.MakeRequest(TrackingRequest.PRODUCTION_URL, ar);

            return td;
        }
    }
}
