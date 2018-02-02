using System;
using SimpleTracking.ShipperInterface.Tracking.Http;

namespace SimpleTracking.ShipperInterface.Common.Http
{
	/// <summary>
	///		This post utility class works above another
	///		"IWebPoster" class to add the ability to retry
	///		the post if the response indicates a failure
	///		that should be retried.
	/// </summary>
	public class PackageTrackrRetryPostUtility : IWebPoster
	{
		private readonly IWebPoster _poster;
		private readonly IThreadSleeper _threadSleeper;

		/// <summary>
		///		Creates a new instance of the <see cref="PackageTrackrRetryPostUtility"/>.
		/// </summary>
		/// <param name="poster">
		///		The base <see cref="IWebPoster"/> that will be retried if necessary.
		/// </param>
		public PackageTrackrRetryPostUtility(IWebPoster poster, IThreadSleeper threadSleeper)
		{
			_poster = poster;
			_threadSleeper = threadSleeper;
		}

		#region Implementation of IWebPoster

		/// <summary>
		///		Posts string data to a URL and receive a string response.
		/// </summary>
		/// <param name="url">
		///		The URL to post the data to.
		/// </param>
		/// <param name="postString">
		///		String data to post to the remote server.
		/// </param>
		/// <returns>
		///		The string response data from the post request.
		/// </returns>
		public string PostData(string url, string postString)
		{
			var tryCount = 0;
			string response;

			do
			{
				tryCount++;

				//Delay the subsequent requests
				if(tryCount > 1)
					_threadSleeper.Sleep(TimeSpan.FromSeconds(1.0));

				response = _poster.PostData(url, postString);
			} while (tryCount < 5 && (response.Length == 0 || response == "-1"));

			return response;
		}

		#endregion
	}
}
