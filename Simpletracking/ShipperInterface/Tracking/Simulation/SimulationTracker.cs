using System;
using System.Collections.Generic;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking.Simulation
{
	/// <summary>
	///		Creates simulated tracking data for testing.
	/// </summary>
	public class SimulationTracker : ITracker
	{
		#region ITracker Members

		/// <summary>
		///		Gets simulated tracking data for certain tracking numbers.
		/// </summary>
		/// <param name="trackingNumber">
		///		sim1: Passes back 2 activities, with basic tracking data.
		///		sim2: Passes back tracking data that has 2 activities that are updated
		///					every hour. This can be used to test RSS feeds or email data.
		/// </param>
		/// <returns></returns>
		public TrackingData GetTrackingData(string trackingNumber)
		{
			var td = new TrackingData();

			if (trackingNumber == "sim1")
				return getSim1TrackingData();
			if (trackingNumber == "sim2")
				return getSim2TrackingData();
			if (trackingNumber == "sim3")
				return getSim3TrackingData();

			return null;
		}

		#endregion

		private static TrackingData getSim1TrackingData()
		{
			var td = new TrackingData();
			td.UsageRequirements = "The simulation tracker is for internal use only";
			td.TrackerName = "Simulation Tracker";

			td.EstimatedDelivery = DateTime.Parse("1-7-15");
			td.Activity = new List<Activity>();

			var activity = new Activity();
			activity.Timestamp = DateTime.Parse("1-6-15 1:30 am");
			activity.ShortDescription = "Picked Up";
			activity.LocationDescription = "Atlanta, GA";
			td.Activity.Add(activity);

			activity = new Activity();
			activity.Timestamp = DateTime.Parse("1-6-15 2:53 pm");
			activity.ShortDescription = "Processed";
			activity.LocationDescription = "Richmond, VA";
			td.Activity.Add(activity);

			return td;
		}

		private static TrackingData getSim2TrackingData()
		{
			var td = new TrackingData();
			td.UsageRequirements = "The simulation tracker is for internal use only";
			td.TrackerName = "Simulation Tracker";

			td.EstimatedDelivery = DateTime.Parse("1-7-15");
			td.Activity = new List<Activity>();

			DateTime baseTime = DateTime.Now;
			baseTime = baseTime.AddMinutes(-baseTime.Minute);

			var activity = new Activity();
			activity.Timestamp = baseTime.AddHours(-1);
			activity.ShortDescription = string.Format("Simulated activity at {0}", activity.Timestamp);
			activity.LocationDescription = "Simulated, SI";
			td.Activity.Add(activity);

			activity = new Activity();
			activity.Timestamp = baseTime;
			activity.ShortDescription = string.Format("Simulated activity at {0}", activity.Timestamp);
			activity.LocationDescription = "Simulated, SI";
			td.Activity.Add(activity);

			return td;
		}

		private static TrackingData getSim3TrackingData()
		{
			throw new ResponseParseException("test response data parse exception");
		}
	}
}