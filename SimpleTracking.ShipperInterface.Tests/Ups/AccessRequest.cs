using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.ShipperInterface.Ups
{
    [TestClass]
    public class AccessRequestTests
    {
        [TestMethod]
        public void Serialize()
        {
            var ar = new AccessRequest("abc", "def", "ghi");
            var serialized = ar.Serialize();

            const string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<AccessRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <AccessLicenseNumber>abc</AccessLicenseNumber>\r\n  <UserId>def</UserId>\r\n  <Password>ghi</Password>\r\n</AccessRequest>";

            Assert.AreEqual(expected, serialized);
        }
    }
}
