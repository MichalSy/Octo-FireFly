using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Octo_FireFly.App.Manager.OctoPrint;
using Octo_FireFly.App.Pages.Counter;
using Octo_FireFly.App.Pages.Dashboard.Models;
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
                return new HttpClient { BaseAddress = new Uri("http://localhost:8080") };
            });

            builder.Services.AddSingleton<IOctoPrintConnector, OctoPrintConnector>();
            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddSingleton<CounterViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
