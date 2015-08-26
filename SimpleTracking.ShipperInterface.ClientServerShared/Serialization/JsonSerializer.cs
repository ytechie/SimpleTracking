using Newtonsoft.Json;

namespace SimpleTracking.ShipperInterface.ClientServerShared.Serialization
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize(object toSerialize)
        {
            return JsonConvert.SerializeObject(toSerialize);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
