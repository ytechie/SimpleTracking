using System;
using System.Xml.Serialization;
using System.Net;
using System.IO;
using System.Text;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.Ups;
using SimpleTracking.ShipperInterface.Tracking;

namespace SimpleTracking.ShipperInterface.Ups.Tracking
{
	[XmlRoot("TrackRequest")]
	public class TrackingRequest
	{
		RequestComponents.Request _req;
		string _trackingNumber;

		public const string DEV_URL = "https://wwwcie.ups.com/ups.app/xml/Track";
		public const string PRODUCTION_URL = "https://www.ups.com/ups.app/xml/Track";

		public TrackingRequest()
		{
		}

		public TrackingRequest(string trackingNumber, string transactionKey)
		{
			_req = new RequestComponents.Request(transactionKey);
			_trackingNumber = trackingNumber;
		}

		[XmlElement("Request")]
		public RequestComponents.Request Request
		{
			get
			{
				return _req;
			}
			set
			{
				_req = value;
			}
		}

		[XmlElement("TrackingNumber")]
		public string TrackingNumber
		{
			get
			{
				return _trackingNumber;
			}
			set
			{
				_trackingNumber = value;
			}
		}

        public TrackingData MakeRequest(AccessRequest ar)
		{
			return MakeRequest(PRODUCTION_URL, ar);
		}

		public TrackingData MakeRequest(string serverUrl, AccessRequest ar)
		{
			string postString;
			byte[] postData;
			HttpWebRequest req;
			Stream requestStream;
			string responseXml;
			WebResponse response;
			Stream responseStream;
			StreamReader sr;

			postString = ar.Serialize();
			postString += Serialize();

			postData = System.Text.Encoding.UTF8.GetBytes(postString);

			//Set up the request
			req = (HttpWebRequest)WebRequest.Create("https://www.ups.com/ups.app/xml/Track");
			req.Method = "POST";
			req.ContentType="application/x-www-form-urlencoded";
			req.ContentLength = postData.Length;
			requestStream = req.GetRequestStream();

			// Send the data.
			requestStream.Write(postData, 0, postData.Length);
			requestStream.Close();

			//Get the response
			response = req.GetResponse();
			responseStream = response.GetResponseStream();
			sr = new StreamReader(responseStream);
			responseXml = sr.ReadToEnd();

			return TrackingResponse.GetCommonTrackingData(responseXml);
		}

		public string Serialize()
		{
			XmlSerializer ser;
			StringWriter writer;
			
			StringBuilder sb;
			
			ser = new XmlSerializer(typeof(TrackingRequest));
			sb = new StringBuilder();
			writer = new System.IO.StringWriter(sb);
			
			ser.Serialize(writer, this);
			writer.Close();

			return sb.ToString();
		}
	}
}
