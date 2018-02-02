using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	/// <summary>
	/// Summary description for ShipTo.
	/// </summary>
	public class ShipTo
	{
		[XmlElement("DestAddress")]
		public Address DestAddress;

		public ShipTo()
		{
		}
	}
}
