using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class PackageMessage
	{
		[XmlElement("Code")]
		public string Code;
    
		[XmlElement("Description")]
		public string Description;
	}
}
