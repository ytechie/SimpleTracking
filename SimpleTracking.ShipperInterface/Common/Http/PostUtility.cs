using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using log4net;

namespace SimpleTracking.ShipperInterface.Tracking.Http
{
	/// <summary>
	///     Provides utilities to make posting an XML blob
	///     to a web address easier.
	/// </summary>
	public class PostUtility : IWebPoster
	{
		/// <summary>
		///		Declare and create our logger
		/// </summary>
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region IWebPoster Members

		/// <summary>
		///		Posts XML to an Internet address and gets the response.
		/// </summary>
		/// <param name="postString">
		///		The string data to post.
		/// </param>
		/// <param name="url">
		///		The URL to post to the data to.
		/// </param>
		/// <returns>
		///		The string response returned by the remote server.
		/// </returns>
		public string PostData(string url, string postString)
		{
			//Set up the request
			var req = (HttpWebRequest)WebRequest.Create(url);

			if (postString != null)
			{
				var postData = Encoding.UTF8.GetBytes(postString);

				req.Method = "POST";
				req.ContentType = "application/x-www-form-urlencoded";
				req.ContentLength = postData.Length;
				using (var requestStream = req.GetRequestStream())
				{
					_log.DebugFormat("Posting '{0}' to '{1}'", postString, url);

					// Send the data.
					requestStream.Write(postData, 0, postData.Length);
					requestStream.Close();
				}
			}

			string responseXml;

			//Get the response
			using (var response = req.GetResponse())
			{
				using (var responseStream = response.GetResponseStream())
				{
					using (var sr = new StreamReader(responseStream))
					{
						responseXml = sr.ReadToEnd();
						_log.DebugFormat("Received {0} characters", responseXml.Length);
					}
				}
			}

			return responseXml;
		}

		#endregion
	}
}