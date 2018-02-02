using System.IO;
using System.Reflection;

namespace SimpleTracking.ShipperInterface.Usps.Tracking
{
	/// <summary>
	///		Methods for working with the XML request needed to call
	///		the USPS API.
	/// </summary>
	public static class TrackingRequest
	{
		/// <summary>
		///		Generates an XML request string that can be used to
		///		request data from the USPS API.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to retreive tracking data for.
		/// </param>
		/// <param name="userName">
		///		The USPS account user name.
		/// </param>
		/// <param name="password">
		///		The USPS account password.
		/// </param>
		/// <returns>
		///		The XML template with the tokens replaced with the supplied values.
		/// </returns>
		public static string GetTrackingRequest(string trackingNumber, string userName, string password)
		{
			string template = readEmbeddedRequestTemplate();

			//Replace the tokens with the real request data
			template = template.Replace("{trackingNumber}", trackingNumber);
			template = template.Replace("{userName}", userName);
			template = template.Replace("{password}", password);

			return template;
		}

		/// <summary>
		///		Reads the embedded request template.
		/// </summary>
		/// <returns>
		///		The embedded request template string.
		/// </returns>
		private static string readEmbeddedRequestTemplate()
		{
			Stream templateStream =
				Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof (TrackingRequest).Namespace + "." +
				                                                          "RequestTemplate.xml");

			using (var sr = new StreamReader(templateStream))
			{
				return sr.ReadToEnd();
			}
		}
	}
}