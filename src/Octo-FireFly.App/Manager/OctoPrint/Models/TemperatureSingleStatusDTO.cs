using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Manager.OctoPrint.Models
{
    public class TemperatureSingleStatusDTO
    {
        [JsonPropertyName("actual")]
        public decimal Actual { get; set; }

        [JsonPropertyName("target")]
        public decimal Target { get; set; }
    }
}
