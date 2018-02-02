using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.RequestComponents
{
	public class TransactionReference
	{
		[XmlElement("XpciVersion")]
		public const string XpciVersion = "1.0001";

		string _customerContext;

		public TransactionReference()
		{
		}

		public TransactionReference(string customerContext)
		{
			_customerContext = customerContext;
		}

		[XmlElement("CustomerContext")]
		public string CustomerContext
		{
			get
			{
				return _customerContext;
			}
			set
			{
				_customerContext = value;
			}
		}
	}
}
