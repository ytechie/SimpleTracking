using System;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	/// <summary>
	///		<see cref="TrackingData"/> that contains extra information
	///		about an error that occurred during tracking the package.
	/// </summary>
	public class ErrorTrackingData : TrackingData
	{
		private readonly Exception _ex;

		/// <summary>
		///		Creates a new instance of the <see cref="ErrorTrackingData"/>.
		/// </summary>
		/// <param name="ex"></param>
		public ErrorTrackingData(Exception ex)
		{
			_ex = ex;
		}

		/// <summary>
		///		Gets the exception that was passed into the constructor.
		/// </summary>
		public Exception Exception
		{
			get { return _ex; }
		}
	}
}