using System;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.RequestComponents
{
	/// <summary>
	/// Summary description for Request.
	/// </summary>
	public class Request
	{
		TransactionReference _tr;
		string _requestAction = "Track";
		string _requestOption = "activity";

		public Request()
		{
		}

		public Request(string contextKey)
		{
			_tr = new TransactionReference(contextKey);
		}

		[XmlElement("TransactionReference")]
		public TransactionReference TransReference
		{
			get
			{
				return _tr;
			}
			set
			{
				_tr = value;
			}
		}

		[XmlElement("RequestAction")]
		public string RequestAction
		{
			get
			{
				return _requestAction;
			}
			set
			{
				_requestAction = value;
			}
		}

		[XmlElement("RequestOption")]
		public string RequestOption
		{
			get
			{
				return _requestOption;
			}
			set
			{
				_requestOption = value;
			}
		}
	}
}
