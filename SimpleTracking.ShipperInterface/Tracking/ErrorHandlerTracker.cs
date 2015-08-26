using System;
using System.Reflection;
using System.Web;
using log4net;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	/// <summary>
	///		Catches exceptions from upstream <see cref="ITracker"/>
	///		instances, and handles them gracefully.
	/// </summary>
	public class ErrorHandlerTracker : ITracker
	{
		/// <summary>
		///		Declare and create our logger.
		/// </summary>
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly ITracker _tracker;

		/// <summary>
		///		Creates a new instance of the <see cref="ErrorHandlerTracker"/> class.
		/// </summary>
		/// <param name="upstreamTracker">
		///		The upstream <see cref="ITracker"/> to attempt to retreive the
		///		tracking information from.
		/// </param>
		public ErrorHandlerTracker(ITracker upstreamTracker)
		{
			_tracker = upstreamTracker;
		}

		#region ITracker Members

		/// <summary>
		///		Tracks the package using the upstream tracker, and returns a blank
		///		result if an error occurs during tracking.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to pass to the upstream tracker.
		/// </param>
		/// <returns>
		///		If the upstream tracker throws and exception, a <see cref="ErrorTrackingData"/>
		///		instance is returned. If the
		///		upstream tracker doesn't throw an exception, the actual
		///		<see cref="TrackingData"/> is returned.
		/// </returns>
		public TrackingData GetTrackingData(string trackingNumber)
		{
			try
			{
				return _tracker.GetTrackingData(trackingNumber);
			}
			catch (ResponseParseException rex)
			{
				//ErrorSignal.FromCurrentContext().Raise(rex);
				_log.Error("An error occurred while processing the response data from a remote call", rex);
				return new ErrorTrackingData(rex);
			}
			catch (Exception ex)
			{
				//if(HttpContext.Current != null)
					//ErrorSignal.FromCurrentContext().Raise(ex);
				_log.Error("An error occurred from an upstream tracker", ex);
				return new ErrorTrackingData(ex);
			}
		}

		#endregion
	}
}