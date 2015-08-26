using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.FedexTrackWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.ShipperInterface.FedEx.Tracking
{
    [TestClass]
    public class AddressExtensionsTests
    {
        [TestMethod]
        public void SerializeAddress_Basic()
        {
            var a = new Address();
            a.City = "GREEN BAY";
            a.StateOrProvinceCode = "WI";
            a.CountryCode = "US";

            Assert.AreEqual("GREEN BAY, WI", a.GetFriendlyAddressString());
        }

        [TestMethod]
        public void SerializeAddress_Empty()
        {
            var a = new Address();

            Assert.AreEqual("", a.GetFriendlyAddressString());
        }
    }
}
