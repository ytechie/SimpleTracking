using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class ReferenceNumber
	{
		[XmlElement("Code")]
		public int Code;
		[XmlElement("Value")]
		public string Value;

		public ReferenceNumber()
		{
		}
	}
}
