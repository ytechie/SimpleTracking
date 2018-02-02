using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class ActivityLocation
	{
		[XmlElement("Address")]
		public Address MyAddress;

		public ActivityLocation()
		{
		}
	}
}
