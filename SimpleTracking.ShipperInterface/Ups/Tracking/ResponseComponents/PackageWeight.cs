using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class PackageWeight
	{
		[XmlElement("Weight")]
		public double Weight;
    
		[XmlElement("UnitOfMeasurement")]
		public UnitOfMeasurement MyUnitOfMeasurement;

	}
}
