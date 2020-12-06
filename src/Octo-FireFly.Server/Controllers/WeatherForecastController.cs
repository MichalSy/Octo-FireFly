using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Octo_FireFly.App.Server.Manager.AutoUpdate;
using Octo_FireFly.App.Shared;
using Octo_FireFly.Server.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAutoUpdater _autoUpdater;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAutoUpdater autoUpdater)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _autoUpdater = autoUpdater ?? throw new ArgumentNullException(nameof(autoUpdater));
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _autoUpdater.CheckForUpdateAsync();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();            
        }
    }
}
