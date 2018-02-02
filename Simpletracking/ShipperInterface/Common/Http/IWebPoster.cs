namespace SimpleTracking.ShipperInterface.Tracking.Http
{
	/// <summary>
	///		An interface for posting data to a remote URL, and receving a response.
	/// </summary>
	public interface IWebPoster
	{
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
		string PostData(string url, string postString);
	}
}