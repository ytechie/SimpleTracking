using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleTracking.ShipperInterface.Tracking
{
    [TestClass]
    public class LocationParserTests
    {
        [TestMethod]
        public void Empty()
        {
            var l = LocationParser.ParseLocation("");

            Assert.AreEqual("", l.State);
        }

        [TestMethod]
        public void StateOnly()
        {
            var l = LocationParser.ParseLocation("GA");

            Assert.AreEqual("GA", l.State);
        }

        [TestMethod]
        public void CityStateHuman()
        {
            var l = LocationParser.ParseLocation("Atlanta, GA");

            Assert.AreEqual("Atlanta", l.City);
            Assert.AreEqual("GA", l.State);
        }

        [TestMethod]
        public void CityStateCountry()
        {
            var l = LocationParser.ParseLocation("Atlanta, GA, US");

            Assert.AreEqual("Atlanta", l.City);
            Assert.AreEqual("GA", l.State);
        }
    }
}
