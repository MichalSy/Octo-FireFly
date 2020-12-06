using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Manager.OctoPrint.Models
{
    public class TemperatureStatusDTO
    {
        [JsonPropertyName("time")]
        public long UnixTime { get; set; }

        [JsonPropertyName("bed")]
        public TemperatureSingleStatusDTO Bed { get; set; }

        [JsonPropertyName("tool0")]
        public TemperatureSingleStatusDTO Tool0 { get; set; }

        [JsonPropertyName("tool1")]
        public TemperatureSingleStatusDTO Tool1 { get; set; }

        [JsonPropertyName("tool2")]
        public TemperatureSingleStatusDTO Tool2 { get; set; }

        [JsonPropertyName("tool3")]
        public TemperatureSingleStatusDTO Tool3 { get; set; }
    }
}
