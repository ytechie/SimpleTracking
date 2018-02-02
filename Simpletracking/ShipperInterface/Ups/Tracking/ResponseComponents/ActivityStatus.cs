using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class ActivityStatus
	{
		[XmlElement("StatusType")]
		public StatusType MyStatusType;
		[XmlElement("StatusCode")]
		public StatusCode MyStatusCode;

		public ActivityStatus()
		{
		}
	}
}
