using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class UnitOfMeasurement
	{
		[XmlElement("Code")]
		public string Code;

		public UnitOfMeasurement()
		{
			
		}
	}
}
