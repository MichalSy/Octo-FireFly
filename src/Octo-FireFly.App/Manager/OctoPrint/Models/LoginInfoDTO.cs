using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Manager.OctoPrint.Models
{
    public class LoginInfoDTO
    {
        [JsonPropertyName("session")]
        public string SessionToken { get; set; }
    }
}
