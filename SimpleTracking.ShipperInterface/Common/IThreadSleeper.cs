using System;

namespace SimpleTracking.ShipperInterface.Common
{
	/// <summary>
	///		Extracts the thread sleep functionality so that its
	///		implementation can vary between testing and production
	///		environments.
	/// </summary>
	public interface IThreadSleeper
	{
		/// <summary>
		///		Signals that the thread should sleep for a period of time.
		/// </summary>
		/// <param name="timeToSleep">
		///		The duration to sleep.
		/// </param>
		void Sleep(TimeSpan timeToSleep);
	}
}
