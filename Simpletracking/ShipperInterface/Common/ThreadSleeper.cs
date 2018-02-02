using System;
using System.Threading;

namespace SimpleTracking.ShipperInterface.Common
{
	/// <summary>
	///		An production implementation of the <see cref="IThreadSleeper"/>
	///		that causes the currently executing thread to sleep.
	/// </summary>
	/// <seealso cref="Thread.Sleep(TimeSpan)"/>
	public class ThreadSleeper : IThreadSleeper
	{
		#region Implementation of IThreadSleeper

		/// <summary>
		///		Causes the currently executing thread to sleep.
		/// </summary>
		/// <param name="timeToSleep">
		///		The duration of time for which the thread should sleep.
		/// </param>
		public void Sleep(TimeSpan timeToSleep)
		{
			Thread.Sleep(timeToSleep);
		}

		#endregion
	}
}
