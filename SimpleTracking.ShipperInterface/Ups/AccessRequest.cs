using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SimpleTracking.ShipperInterface.Ups
{
	/// <summary>
	/// Summary description for AccessRequest.
	/// </summary>
	public class AccessRequest
	{
		string _licenseNumber;
		string _userName;
		string _password;

        /// <summary>
        ///     A contstructor for the serializer
        /// </summary>
        public AccessRequest()
        {
        }

		public AccessRequest(string licenseNumber, string userName, string password)
		{
			_licenseNumber = licenseNumber;
			_userName = userName;
			_password = password;
		}

		[XmlElement("AccessLicenseNumber")]
		public string LicenseNumber
		{
			get
			{
				return _licenseNumber;
			}
			set
			{
				_licenseNumber = value;
			}
		}

		[XmlElement("UserId")]
		public string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				_userName = value;
			}
		}

		[XmlElement("Password")]
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		public string Serialize()
		{
            var ser = new XmlSerializer(typeof(AccessRequest));
            var sb = new StringBuilder();
            using (var writer = new System.IO.StringWriter(sb))
            {
                ser.Serialize(writer, this);
                writer.Close();
            }

            return sb.ToString();
		}
	}
}
