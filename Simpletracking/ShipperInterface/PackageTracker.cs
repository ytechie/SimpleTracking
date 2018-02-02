using System.Collections.Generic;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Geocoding;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;
using SimpleTracking.ShipperInterface.Ups.Tracking;
using SimpleTracking.ShipperInterface.Usps.Tracking;

namespace SimpleTracking.ShipperInterface
{
    public class PackageTracker : ITracker
    {
        private ITracker _defaultTracker;

        public PackageTracker(IWebPoster webPoster, IGeocodeDb geocodeDb)
        {
            //Todo: This is a bad place to load these:
            var uspsUserName = "";
            var uspsPassword = "";

            var fedexKey = "";
            var fedexPassword = "";
            var fedexAccountNumber = "";
            var fedexMeterNumber = "";


            var coreTrackers = new List<ITracker>();
            coreTrackers.Add(new Tracking.Simulation.SimulationTracker());
            coreTrackers.Add(new UpsTracker());
            coreTrackers.Add(new UspsTracker(new PostUtility(), uspsUserName, uspsPassword, true));
            coreTrackers.Add(new FedexTracker(new TrackService(), fedexKey, fedexPassword, fedexAccountNumber, fedexMeterNumber, false));
            //coreTrackers.Add(new DhlTracker(new PostUtility(), "", "");

            var multiTracker = new MultiTracker(coreTrackers);
            var cacheTracker = new CacheTracker(multiTracker);
            var emptyTracker = new EmptyTrackingNumberTracker(cacheTracker);
            var loggingTracker = new LoggingTracker(emptyTracker);
            var sanitizerTracker = new TrackingNumberStandardizerTracker(loggingTracker);
            var geocodingTracker = new GeocodingTracker(sanitizerTracker, geocodeDb);
            var errorHandlerTracker = new ErrorHandlerTracker(geocodingTracker);

            _defaultTracker = errorHandlerTracker;
        }

        public TrackingData GetTrackingData(string trackingNumber)
        {
            return _defaultTracker.GetTrackingData(trackingNumber);
        }
    }
 }
