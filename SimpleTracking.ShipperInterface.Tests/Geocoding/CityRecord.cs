using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleTracking.ShipperInterface.Geocoding
{
    [TestClass]
    public class CityRecordTests
    {
        [TestMethod]
        public void ParseCityRecord()
        {
            const string recordString = "00624,PENUELAS,PR,18.058333,66.721944,PENUELAS";

            var cr = new CityRecord(recordString);
            Assert.AreEqual("00624", cr.Zip);
            Assert.AreEqual("PENUELAS", cr.City);
            Assert.AreEqual("PR", cr.State);
            Assert.AreEqual(18.058333, cr.Latitude);
            Assert.AreEqual(-66.721944, cr.Longitude);
            Assert.AreEqual("PENUELAS", cr.County);
        }
    }
}
