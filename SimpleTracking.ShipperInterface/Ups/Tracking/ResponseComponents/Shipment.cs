using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	/// <summary>
	/// Summary description for Shipment.
	/// </summary>
	public class Shipment
	{
		[XmlElement("Shipper")]
		public Shipper MyShipper;
		[XmlElement("ShipTo")]
		public ShipTo MyShipTo;
		[XmlElement("Service")]
		public Service MyService;
		[XmlElement("ReferenceNumber")]
		public ReferenceNumber[] MyReferenceNumbers;
		[XmlElement("ShipmentIdentificationNumber")]
		public string ShipmentIdentificationNumber;
		[XmlElement("PickupDate")]
		public string PickupDate;
		[XmlElement("ScheduledDeliveryDate")]
		public string ScheduledDeliveryDate;
		[XmlElement("Package")]
		public Package MyPackage;

		public Shipment()
		{

		}
	}
}
