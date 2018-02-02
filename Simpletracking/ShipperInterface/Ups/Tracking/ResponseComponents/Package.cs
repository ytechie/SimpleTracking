using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	/// <summary>
	/// Summary description for Package.
	/// </summary>
	public class Package
	{
			[XmlElement("TrackingNumber")]
			public string TrackingNumber;
    
			//[XmlArray, XmlArrayItem("Activity", typeof(PackageActivity))]

			[XmlElement("Activity")]
			public PackageActivity[] Activity;
    
			[XmlElement("Message")]
			public PackageMessage Message;
    
			[XmlElement("PackageWeight")]
			public PackageWeight PackageWeight;
		
	}
}
