using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Manager.OctoPrint.Models
{
    public class PrinterStatusDTO
    {
        [JsonPropertyName("temps")]
        public IEnumerable<TemperatureStatusDTO> TemperatureStatus { get; set; }
    }
}
