using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	/// <summary>
	///		An ITracker that returns empty tracking data if the
	///		supplied tracking number is NULL or an empty string.
	/// </summary>
	public class EmptyTrackingNumberTracker : ITracker
	{
		private readonly ITracker _upstreamTracker;

		/// <summary>
		///		Creates a new instance of the <see cref="EmptyTrackingNumberTracker"/>.
		/// </summary>
		/// <param name="upstreamTracker">
		///		The upstream tracker to use if the tracking number is
		///		not NULL or an empty string.
		/// </param>
		public EmptyTrackingNumberTracker(ITracker upstreamTracker)
		{
			_upstreamTracker = upstreamTracker;
		}

		#region Implementation of ITracker

		/// <summary>
		///		Gets the tracking information for the specified tracking number.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to retrieve tracking data for.
		/// </param>
		/// <returns>
		///		The <see cref="TrackingData" /> instance that contains the
		///		tracking information for the tracking number. If the tracking
		///		information cannot be retrieved with this tracker, NULL is
		///		returned.
		/// </returns>
		public TrackingData GetTrackingData(string trackingNumber)
		{
			if(string.IsNullOrEmpty(trackingNumber) || trackingNumber.Trim().Length == 0)
				return new TrackingData();

			return _upstreamTracker.GetTrackingData(trackingNumber);
		}

		#endregion
	}
}
