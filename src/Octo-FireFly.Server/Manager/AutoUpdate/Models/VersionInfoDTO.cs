using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Server.Manager.AutoUpdate.Models
{
    public class VersionInfoDTO
    {
        [JsonPropertyName("tag_name")]
        public string TagName { get; set; }

        public IEnumerable<VersionInfoAssetDTO> Assets { get; set; } = Array.Empty<VersionInfoAssetDTO>();
    }

    public class VersionInfoAssetDTO
    {
        public string Name { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string DownloadUrl { get; set; }
    }
}
