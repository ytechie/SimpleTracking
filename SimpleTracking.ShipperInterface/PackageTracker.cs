using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Common;
using SimpleTracking.ShipperInterface.Geocoding;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;
using SimpleTracking.ShipperInterface.Ups.Tracking;
using SimpleTracking.ShipperInterface.Usps.Tracking;
using SimpleTracking.ShipperInterface.FedEx.Tracking;
using SimpleTracking.ShipperInterface.Dhl.Tracking;
using SimpleTracking.ShipperInterface.FedexTrackWebService;
using System.Web.Configuration;

namespace SimpleTracking.ShipperInterface
{
    public class PackageTracker : ITracker
    {
        private ITracker _defaultTracker;

        public PackageTracker(IWebPoster webPoster, IGeocodeDb geocodeDb)
        {
            //Todo: This is a bad place to load these:
            var uspsUserName = WebConfigurationManager.AppSettings["UspsUserName"];
            var uspsPassword = WebConfigurationManager.AppSettings["UspsPassword"];

            var fedexKey = WebConfigurationManager.AppSettings["FedexKey"];
            var fedexPassword = WebConfigurationManager.AppSettings["FedexPassword"];
            var fedexAccountNumber = WebConfigurationManager.AppSettings["FedexAccountNumber"];
            var fedexMeterNumber = WebConfigurationManager.AppSettings["FedexMeterNumber"];


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
