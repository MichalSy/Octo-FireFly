using Microsoft.Extensions.Logging;
using Octo_FireFly.App.Common;
using Octo_FireFly.App.Manager.Temperature;
using System;

namespace Octo_FireFly.App.Pages.Print.Models
{
    public class PrintPageViewModel : ViewModelBase
    {
        private readonly ILogger<PrintPageViewModel> _logger;
        private readonly ITemperatureManager _temperatureManager;

        public decimal CurrentToolActual { get; set; }

        public decimal CurrentToolTarget { get; set; }

        public decimal CurrentBedActual { get; set; }
        public decimal CurrentBedTarget { get; set; }

        public PrintPageViewModel(ILogger<PrintPageViewModel> logger, ITemperatureManager temperatureManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _temperatureManager = temperatureManager ?? throw new ArgumentNullException(nameof(temperatureManager));
        }

        private void OnTemperatureChangedExecute(object sender, EventArgs e)
        {
            CurrentToolActual = _temperatureManager.CurrentToolActual;
            CurrentToolTarget = _temperatureManager.CurrentToolTarget;
            CurrentBedActual = _temperatureManager.CurrentBedActual;
            CurrentBedTarget = _temperatureManager.CurrentBedTarget;

            _logger.LogInformation($"{_temperatureManager.CurrentToolActual}");

            OnUIChanged?.Invoke();
        }

        public override void OnPageShown()
        {
            _logger.LogInformation("SHOWN");
            _temperatureManager.OnTemperatureChanged += OnTemperatureChangedExecute;
        }

        public override void OnPageHide()
        {
            _logger.LogInformation("HIDE");
            _temperatureManager.OnTemperatureChanged -= OnTemperatureChangedExecute;
        }
    }
}
