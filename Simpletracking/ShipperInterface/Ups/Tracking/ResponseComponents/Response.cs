using System;
using System.Xml.Serialization;

using SimpleTracking.ShipperInterface.Ups.Tracking.RequestComponents;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class Response
	{
		TransactionReference _tr;
		int _responseStatusCode;
		string _responseStatusDescription;

		public Response()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[XmlElement("TransactionReference")]
		public TransactionReference TransRef
		{
			get
			{
				return _tr;
			}
			set
			{
				_tr = value;
			}
		}

		[XmlElement("ResponseStatusCode")]
		public int ResponseStatusCode
		{
			get
			{
				return _responseStatusCode;
			}
			set
			{
				_responseStatusCode = value;
			}
		}

		[XmlElement("ResponseStatusDescription")]
		public string ResponseStatusDescription
		{
			get
			{
				return _responseStatusDescription;
			}
			set
			{
				_responseStatusDescription = value;
			}
		}
	}
}
