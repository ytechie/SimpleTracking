using System;
using System.Text;

namespace SimpleTracking.ShipperInterface.Ups.Tracking.ResponseComponents
{
	public class Address
	{
		[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string AddressLine1;
    
		[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string City;
    
		[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string StateProvinceCode;
    
		[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string PostalCode;
    
		[System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string CountryCode;

		public override string ToString()
		{
			bool firstComponent = true;
			StringBuilder s;

			s = new StringBuilder();

			if(AddressLine1 != null)
			{
				s.Append(AddressLine1);
				firstComponent = false;
			}
			if(City != null)
			{
				if(!firstComponent)
					s.Append(", ");

				s.Append(City);
				firstComponent = false;
			}
			if(StateProvinceCode != null)
			{
				if(!firstComponent)
					s.Append(", ");

				s.Append(StateProvinceCode);
				firstComponent = false;
			}
			if(PostalCode != null)
			{
				if(!firstComponent)
					s.Append(", ");

				s.Append(PostalCode);
				firstComponent = false;
			}
			if(CountryCode != null)
			{
				if(!firstComponent)
					s.Append(", ");

				s.Append(CountryCode);
				firstComponent = false;
			}

			return s.ToString();
		}
	}
}
