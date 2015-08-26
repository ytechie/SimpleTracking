using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
    [TestClass]
    public class TrackingNumberStandardizerTrackerTests
    {
        [TestMethod]
        public void UseDownstreamTracker()
        {
            var downstreamTracker = MockRepository.GenerateMock<ITracker>();
            var expectedTrackingData = new TrackingData();
            downstreamTracker.Expect(x => x.GetTrackingData("abc")).Return(expectedTrackingData);

            var t = new TrackingNumberStandardizerTracker(downstreamTracker);
            var actualTrackingData = t.GetTrackingData("abc");

            Assert.AreEqual(expectedTrackingData, actualTrackingData);
        }

        [TestMethod]
        public void UsesTrackingNumberUtilities()
        {
            var downstreamTracker = MockRepository.GenerateMock<ITracker>();
            var expectedTrackingData = new TrackingData();
            downstreamTracker.Expect(x => x.GetTrackingData("abc123")).Return(expectedTrackingData);

            var t = new TrackingNumberStandardizerTracker(downstreamTracker);
            var actualTrackingData = t.GetTrackingData("abc 123");

            downstreamTracker.VerifyAllExpectations();
        }
    }
}
