using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking.Simulation
{
	[TestClass]
	public class SimulationTracker_Tester
	{
		private SimulationTracker _st;

		[TestMethod]
		public void Invalid_Simulation_Number_Verify_Null_Tracking_Data()
		{
			_st = new SimulationTracker();
			TrackingData td = _st.GetTrackingData("simz");
			Assert.IsNull(td);
		}

		[TestMethod]
		public void Track_sim1_Verify_Tracking_Data()
		{
			_st = new SimulationTracker();
			TrackingData td = _st.GetTrackingData("sim1");
			Assert.AreEqual(2, td.Activity.Count);
		}

		[TestMethod]
		public void Track_sim2_Verify_Tracking_Data()
		{
			_st = new SimulationTracker();
			TrackingData td = _st.GetTrackingData("sim2");
			Assert.AreEqual(2, td.Activity.Count);

			DateTime now = DateTime.Now;

			Assert.AreEqual(now.Date, td.Activity[0].Timestamp.Date);
			Assert.AreEqual(now.Hour - 1, td.Activity[0].Timestamp.Hour);
			Assert.AreEqual(0, td.Activity[0].Timestamp.Minute);

			Assert.AreEqual(now.Date, td.Activity[1].Timestamp.Date);
			Assert.AreEqual(now.Hour, td.Activity[1].Timestamp.Hour);
			Assert.AreEqual(0, td.Activity[1].Timestamp.Minute);
		}
	}
}