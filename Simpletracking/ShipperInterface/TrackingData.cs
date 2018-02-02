using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTracking.ShipperInterface.ClientServerShared
{
	/// <summary>
	///		Contains the tracking information for a particular package.
	/// </summary>
	public class TrackingData
	{
		/// <summary>
		///		Creates a new instance of the <see cref="TrackingData"/> class.
		/// </summary>
		/// <remarks>
		///		The <see cref="Activity"/> list is created in this constructor
		///		to avoid NULL issues later.
		/// </remarks>
		public TrackingData()
		{
			Activity = new List<Activity>();
            ReferenceNumbers = new List<string>();
		}

		/// <summary>
		///		The date that the package is estimated to be delivered on.
		/// </summary>
		public DateTime? EstimatedDelivery { get; set; }

		/// <summary>
		///		Gets a list of steps that the package has gone
		///		through to reach its destination.
		/// </summary>
		public List<Activity> Activity { get; set; }

		/// <summary>
		///		Gets the user friendly name for a tracker. For example,
		///		UPS or FedEx.
		/// </summary>
		public string TrackerName { get; set; }

		/// <summary>
		///		Gets information about the usage requirements for this tracker.
		///		Display this to comply with EULAs.
		/// </summary>
		public string UsageRequirements { get; set; }

        public List<string> ReferenceNumbers { get; private set; }

        /// <summary>
        ///     Ground, etc.
        /// </summary>
        public string ServiceType { get; set; }

        public decimal? Weight { get; set; }

        public DateTime? LastUpdated { get; set; }

	    public ShipmentStage? GetCurrentShipmentStage()
	    {
	        if (Activity == null || Activity.Count == 0)
	            return null;

	        var lastActivity = Activity.OrderByDescending(x => x.Timestamp).FirstOrDefault();
	        if (lastActivity == null)
	            return null;

	        return lastActivity.Stage;
	    }

	    public string GetCurrentActivityStatus()
	    {
            if (Activity == null || Activity.Count == 0)
                return null;

            var lastActivity = Activity.OrderByDescending(x => x.Timestamp).FirstOrDefault();
            if (lastActivity == null)
                return null;

            return lastActivity.ShortDescription;
	    }
	}
}