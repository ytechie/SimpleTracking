using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Geocoding;

namespace SimpleTracking.ShipperInterface.Tracking
{
    [TestClass]
    public class GeocodingTrackerTests
    {
        [TestMethod]
        public void Test()
        {
            var baseTracker = A.Fake<ITracker>();
            var geocodeDb = A.Fake<IGeocodeDb>();

            var baseTrackingData = new TrackingData();
            baseTrackingData.Activity.Add(new Activity {LocationDescription = "Green Bay, WI"});
            baseTrackingData.Activity.Add(new Activity {LocationDescription = "Seattle, WA"});

            A.CallTo(() => baseTracker.GetTrackingData("abc")).Returns(baseTrackingData);
            A.CallTo(() => geocodeDb.GetCity("Green Bay", "WI")).Returns(new CityRecord() {Latitude = 10, Longitude = 11});
            A.CallTo(() => geocodeDb.GetCity("Seattle", "WA")).Returns(new CityRecord() { Latitude = 12, Longitude = 13 });

            var gt = new GeocodingTracker(baseTracker, geocodeDb);
            
            var td = gt.GetTrackingData("abc");
            Assert.AreEqual(10, td.Activity[0].Latitude);
            Assert.AreEqual(11, td.Activity[0].Longitude);
            Assert.AreEqual(12, td.Activity[1].Latitude);
            Assert.AreEqual(13, td.Activity[1].Longitude);
        }
    }
}
