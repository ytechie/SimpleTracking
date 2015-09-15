using SimpleTracking.ShipperInterface;
using SimpleTracking.ShipperInterface.Geocoding;
using SimpleTracking.ShipperInterface.Tracking;
using SimpleTracking.ShipperInterface.Tracking.Http;
using StructureMap;
using System.Web.Configuration;

namespace SimpleTracking.Web
{
    public static class IocConfiguration
    {
        public static void Configure()
        {
            var geocodingDbConnectionString = WebConfigurationManager.AppSettings["GeocodeDbConnectionString"];

            ObjectFactory.Initialize(x =>
            {
                x.For<IWebPoster>().Use<PostUtility>();
                x.For<ITracker>().Use<PackageTracker>();
                x.For<IGeocodeDb>().Use<GeocodeDb>().Ctor<string>("geocodeDbConnectionString").Is(geocodingDbConnectionString);
            });
        }
    }
}