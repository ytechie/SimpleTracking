using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	/// <summary>
	/// Summary description for Shipper.
	/// </summary>
	public class Shipper
	{
		[XmlElement("ShipperNumber")]
		public string ShipperNumber;
		[XmlElement("SourceAddress")]
		public Address SourceAddress;

		public Shipper()
		{
		}
	}
}
