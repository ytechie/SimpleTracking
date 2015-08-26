using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracking.ShipperInterface.ClientServerShared.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object toSerialize);
        T Deserialize<T>(string json);
    }
}
