using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	[TestClass]
	public class ErrorHandlerTracker_Tester
	{
		#region Setup/Teardown

		[TestInitialize]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_mockTracker = _mocks.CreateMock<ITracker>();
		}

		#endregion

		private ErrorHandlerTracker _eht;

		private MockRepository _mocks;
		private ITracker _mockTracker;

		[TestMethod]
		public void Pass_On_Valid_Tracking_Data()
		{
			var td = new TrackingData();
			_eht = new ErrorHandlerTracker(_mockTracker);

			Expect.Call(_mockTracker.GetTrackingData("Pass_On_Valid_Tracking_Data")).Return(td);

			_mocks.ReplayAll();

			Assert.AreEqual(td, _eht.GetTrackingData("Pass_On_Valid_Tracking_Data"));

			_mocks.VerifyAll();
		}

		[TestMethod]
		public void Suppress_Upstream_Exception()
		{
			var testException = new Exception("test");

			_eht = new ErrorHandlerTracker(_mockTracker);

			Expect.Call(_mockTracker.GetTrackingData("Suppress_Upstream_Exception")).Throw(testException);

			_mocks.ReplayAll();

			TrackingData td = _eht.GetTrackingData("Suppress_Upstream_Exception");
			Assert.AreEqual(true, td is ErrorTrackingData);
			Assert.AreEqual(testException, ((ErrorTrackingData) td).Exception);

			_mocks.VerifyAll();
		}
	}
}