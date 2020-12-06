using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Octo_FireFly.App.Manager.OctoPrint;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Octo_FireFly.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton(sp =>
            {
                return new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            });

            builder.Services.AddSingleton<IOctoPrintConnector, OctoPrintConnector>();

            await builder.Build().RunAsync();
        }
    }
}
