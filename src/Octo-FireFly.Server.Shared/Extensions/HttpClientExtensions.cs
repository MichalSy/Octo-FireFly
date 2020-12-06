using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Octo_FireFly.Server.Shared.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> SendPostAsync<T>(this HttpClient httpClient, string uri, object sendObject)
        {
            var content = JsonSerializer.Serialize(sendObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            var response = await httpClient.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
            var responseText = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseText);
        }
    }
}
