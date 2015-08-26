using System;
using System.Collections.Generic;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	/// <summary>
	///		Asynchronously queries multiple trackers, and returns the
	///		tracking data for the one that is able to track the tracking
	///		number.
	/// </summary>
	public class MultiTracker : ITracker
	{
		private readonly List<ITracker> _trackers;

		/// <summary>
		///		Creates a new instance of the <see cref="MultiTracker"/> class.
		/// </summary>
		/// <param name="trackers"></param>
		public MultiTracker(IEnumerable<ITracker> trackers)
		{
			_trackers = new List<ITracker>(trackers);
		}

		#region ITracker Members

		/// <summary>
		///		Gets the tracking information from the child trackers, and passes
		///		back the tracking data from the first one to respond with data.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to request tracking data from the upstream trackers.
		/// </param>
		/// <returns>
		///		If one of the upstream trackers returns data, the first one in the list with
		///		non-null data will have it's data returned. If none of the upstream trackers
		///		returns data, NULL will be returned.
		/// </returns>
		public TrackingData GetTrackingData(string trackingNumber)
		{
			var trackerDelegates = new List<getTrackDataDelegate>();
			var asyncResults = new List<IAsyncResult>();
			TrackingData trackingData = null;

			foreach (ITracker currTracker in _trackers)
			{
				getTrackDataDelegate currDelegate = currTracker.GetTrackingData;
				trackerDelegates.Add(currDelegate);

				IAsyncResult result = currDelegate.BeginInvoke(trackingNumber, null, null);
				asyncResults.Add(result);
			}

			for (int i = 0; i < trackerDelegates.Count; i++)
			{
				TrackingData td = trackerDelegates[i].EndInvoke(asyncResults[i]);
				if (trackingData == null)
					trackingData = td;
			}

			return trackingData;
		}

		#endregion

		#region Nested type: getTrackDataDelegate

		private delegate TrackingData getTrackDataDelegate(string trackingNumber);

		#endregion
	}
}