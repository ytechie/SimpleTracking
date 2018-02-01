using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	/// <summary>
	///		Provides an interface that allows the different tracking
	///		interfaces to be used interchangably once created.
	/// </summary>
	public interface ITracker
	{
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
		TrackingData GetTrackingData(string trackingNumber);
	}
}