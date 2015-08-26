using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	[TestClass]
	public class MultiTracker_Tester
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_mt1 = _mocks.CreateMock<ITracker>();
			_mt2 = _mocks.CreateMock<ITracker>();
		}

		#endregion

		private MultiTracker _mt;

		private MockRepository _mocks;

		private ITracker _mt1;
		private ITracker _mt2;

		[TestMethod]
		public void One_Tracker_Verify_Pass_Through()
		{
			var td = new TrackingData();

			Expect.Call(_mt1.GetTrackingData("abc")).Return(td);

			_mocks.ReplayAll();

			_mt = new MultiTracker(new[] {_mt1});
			Assert.AreEqual(td, _mt.GetTrackingData("abc"));

			_mocks.VerifyAll();
		}

		[TestMethod]
		public void Two_Trackers_Both_Work_Verify_Return_First()
		{
			var td = new TrackingData();
			var td2 = new TrackingData();

			Expect.Call(_mt1.GetTrackingData("abc")).Return(td);
			Expect.Call(_mt2.GetTrackingData("abc")).Return(td2);

			_mocks.ReplayAll();

			_mt = new MultiTracker(new[] {_mt1, _mt2});
			Assert.AreEqual(td, _mt.GetTrackingData("abc"));

			_mocks.VerifyAll();
		}

		[TestMethod]
		public void Two_Trackers_Neither_Work_Verify_Return_Null()
		{
			Expect.Call(_mt1.GetTrackingData("abc")).Return(null);
			Expect.Call(_mt2.GetTrackingData("abc")).Return(null);

			_mocks.ReplayAll();

			_mt = new MultiTracker(new[] {_mt1, _mt2});
			Assert.AreEqual(null, _mt.GetTrackingData("abc"));

			_mocks.VerifyAll();
		}

		[TestMethod]
		public void Two_Trackers_One_Works_Verify_Pass_Through()
		{
			var td = new TrackingData();

			Expect.Call(_mt1.GetTrackingData("abc")).Return(td);
			Expect.Call(_mt2.GetTrackingData("abc")).Return(null);

			_mocks.ReplayAll();

			_mt = new MultiTracker(new[] {_mt1, _mt2});
			Assert.AreEqual(td, _mt.GetTrackingData("abc"));

			_mocks.VerifyAll();
		}

		[TestMethod]
		public void Two_Trackers_One_Works_Verify_Pass_Through_2()
		{
			var td = new TrackingData();

			Expect.Call(_mt1.GetTrackingData("abc")).Return(null);
			Expect.Call(_mt2.GetTrackingData("abc")).Return(td);

			_mocks.ReplayAll();

			_mt = new MultiTracker(new[] {_mt1, _mt2});
			Assert.AreEqual(td, _mt.GetTrackingData("abc"));

			_mocks.VerifyAll();
		}
	}
}