using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FcuCore {
    public class CommunicationSettings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TransportProviders
        {
            Serial,
            Test
        }

        public TransportProviders Transport { get; set; } = TransportProviders.Serial;

        public string SerialPort { get; set; } = "";
    }
}