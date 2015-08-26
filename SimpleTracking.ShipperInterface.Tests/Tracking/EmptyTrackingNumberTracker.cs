using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	[TestClass]
	public class EmptyTrackingNumberTracker_Tester
	{
		private ITracker _mockUpstreamTracker;
		private EmptyTrackingNumberTracker _t;

		[TestInitialize]
		public void SetUp()
		{
			_mockUpstreamTracker = MockRepository.GenerateStub<ITracker>();
			_t = new EmptyTrackingNumberTracker(_mockUpstreamTracker);
		}

		[TestCleanup]
		public void TearDown()
		{
			_mockUpstreamTracker.VerifyAllExpectations();
		}
		
		[TestMethod]
		public void Null_Tracking_Number_Verify_Empty_Tracking_Data()
		{
			var td = _t.GetTrackingData(null);
			Assert.AreEqual(0, td.Activity.Count);
		}

		[TestMethod]
		public void Empty_Tracking_Number_Verify_Empty_Tracking_Data()
		{
			var td = _t.GetTrackingData("");
			Assert.AreEqual(0, td.Activity.Count);
		}

		[TestMethod]
		public void Empty_Tracking_Number_Verify_Empty_Tracking_Data2()
		{
			var td = _t.GetTrackingData(" ");
			Assert.AreEqual(0, td.Activity.Count);
		}

		[TestMethod]
		public void Non_Empty_Tracking_Number_Verify_Upstream_Tracker_Used()
		{
			var expectedTrackingData = new TrackingData();
			_mockUpstreamTracker.Expect(x => x.GetTrackingData("abc")).Return(expectedTrackingData);
			var td = _t.GetTrackingData("abc");
			Assert.AreEqual(td, expectedTrackingData);
		}
	}
}
