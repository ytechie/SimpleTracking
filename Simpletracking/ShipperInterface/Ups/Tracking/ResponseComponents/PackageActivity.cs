using System;
using System.Xml.Serialization;
using System.Globalization;
using SimpleTracking.ShipperInterface.Ups.Tracking;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class PackageActivity
	{
		DateTime _timestamp;
		
		[XmlElement("ActivityLocation")]
		public ActivityLocation Location;
    
		[XmlElement("Status")]
		public ActivityStatus Status;

		[XmlElement("Date")]
		public string DateString
		{
			get
			{
				throw new NotImplementedException("");
			}
			set
			{
				DateTime newDate;

				newDate = UpsFormatConversions.parseUpsDate(value);
				_timestamp = new DateTime(newDate.Year, newDate.Month, newDate.Day, _timestamp.Hour,
					_timestamp.Minute, _timestamp.Second, _timestamp.Millisecond);
			}
		}

		[XmlElement("Time")]
		public string TimeString
		{
			get
			{
				throw new NotImplementedException("");
			}
			set
			{
				DateTime newTime;

				newTime = UpsFormatConversions.parseUpsTime(value);
				_timestamp = new DateTime(_timestamp.Year, _timestamp.Month, _timestamp.Day, newTime.Hour,
					newTime.Minute, newTime.Second, newTime.Millisecond);
			}
		}

		public DateTime Timestamp
		{
			get{return _timestamp;}
			set{_timestamp = value;}
		}	
	}
}
