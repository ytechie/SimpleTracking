using System;

namespace SimpleTracking.ShipperInterface.Tracking
{
	public class ResponseParseException : ShipperInterfaceException
	{
		public string ResponseData { get; set; }

		public ResponseParseException(string responseData)
		{
			ResponseData = responseData;
		}

		public ResponseParseException(string responseData, Exception innerException) : base(null, innerException)
		{
			ResponseData = responseData;
		}
	}
}
