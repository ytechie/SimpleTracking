using System.IO;
using System.Reflection;

namespace SimpleTracking.ShipperInterface.Dhl.Tracking
{
	/// <summary>
	///     A class used to generate the request XML to track
	///     a DHL package.
	/// </summary>
	public static class TrackingRequest
	{
		private static string getTrackingRequestTemplate()
		{
			Stream readStream;
			StreamReader reader;
			Assembly asm;
			string manifestName;

			asm = typeof (TrackingRequest).Assembly;
			manifestName =
				string.Format("{0}.{1}", typeof (TrackingRequest).Namespace + ".RequestComponents", "TrackingRequestTemplate.xml");
			readStream = asm.GetManifestResourceStream(manifestName);
			reader = new StreamReader(readStream);

			return reader.ReadToEnd();
		}

		public static string GetTrackingRequest(string trackingNumber, string id, string password)
		{
			string template = getTrackingRequestTemplate();

			//Replace the tokens with the real request data
			template = template.Replace("{trackingNumber}", trackingNumber);
			template = template.Replace("{id}", id);
			template = template.Replace("{password}", password);

			return template;
		}
	}
}