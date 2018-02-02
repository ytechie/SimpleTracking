using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
    public class TrackingNumberStandardizerTracker : ITracker
    {
        private ITracker _downstreamTracker;

        public TrackingNumberStandardizerTracker(ITracker downstreamTracker)
        {
            _downstreamTracker = downstreamTracker;
        }

        public TrackingData GetTrackingData(string trackingNumber)
        {
            var standardized = TrackingNumberUtilities.CleanTrackingNumber(trackingNumber, true);

            return _downstreamTracker.GetTrackingData(standardized);
        }
    }
}
