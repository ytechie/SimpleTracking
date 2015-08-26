using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	[TestClass]
	public class CacheTracker_Tester
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_mockTracker = _mocks.CreateMock<ITracker>();

			CacheTracker.ClearCache();
		}

		#endregion

		private CacheTracker _ct;

		private MockRepository _mocks;
		private ITracker _mockTracker;

		[TestMethod]
		public void Cache_Expired_Recheck_Tracker()
		{
			var td = new TrackingData();

			Expect.Call(_mockTracker.GetTrackingData("abc")).Return(td).Repeat.Twice();

			_mocks.ReplayAll();

			_ct = new CacheTracker(_mockTracker, TimeSpan.FromSeconds(1.0));
			_ct.GetTrackingData("abc"); //This should use the passed in tracker
			Thread.Sleep(2000);
			_ct.GetTrackingData("abc"); //This should use ask the pass in tracker again

			_mocks.VerifyAll();
		}

		[TestMethod]
		public void Simple_Cache_Check()
		{
			var td = new TrackingData();

			Expect.Call(_mockTracker.GetTrackingData("abc")).Return(td);

			_mocks.ReplayAll();

			_ct = new CacheTracker(_mockTracker, TimeSpan.FromSeconds(10.0));
			_ct.GetTrackingData("abc"); //This should use the passed in tracker
			_ct.GetTrackingData("abc"); //This should use the cache

			_mocks.VerifyAll();
		}

		[TestMethod]
		public void Try_Default_Cache_Time()
		{
			var td = new TrackingData();

			Expect.Call(_mockTracker.GetTrackingData("abc")).Return(td);

			_mocks.ReplayAll();

			_ct = new CacheTracker(_mockTracker);
			_ct.GetTrackingData("abc"); //This should use the passed in tracker
			_ct.GetTrackingData("abc"); //This should use the cache

			_mocks.VerifyAll();
		}

		[TestMethod]
		public void No_Tracking_Data_Available()
		{
			var td = new TrackingData();

			Expect.Call(_mockTracker.GetTrackingData("abc")).Return(null).Repeat.Twice();

			_mocks.ReplayAll();

			_ct = new CacheTracker(_mockTracker);
			Assert.AreEqual(null, _ct.GetTrackingData("abc"));
			Assert.AreEqual(null, _ct.GetTrackingData("abc"));

			_mocks.VerifyAll();
		}
	}
}