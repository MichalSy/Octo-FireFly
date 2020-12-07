using Microsoft.Extensions.Logging;
using Octo_FireFly.App.Common;
using Octo_FireFly.App.Manager.OctoPrint;
using Octo_FireFly.App.Manager.OctoPrint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Pages.Dashboard.Models
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly ILogger<DashboardViewModel> _logger;
        private readonly IOctoPrintConnector _octoPrintConnector;

        public decimal CurrentToolActual { get; set; }

        public decimal CurrentToolTarget { get; set; }

        public decimal CurrentBedActual { get; set; }
        public decimal CurrentBedTarget { get; set; }

        public DashboardViewModel(ILogger<DashboardViewModel> logger, IOctoPrintConnector octoPrintConnector)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _octoPrintConnector = octoPrintConnector ?? throw new ArgumentNullException(nameof(octoPrintConnector));
        }

        private void OnTemperatureChangedExecute(object sender, TemperatureStatusDTO e)
        {
            CurrentToolActual = e.Tool0.Actual;
            CurrentToolTarget = e.Tool0.Target;
            CurrentBedActual = e.Bed.Actual;
            CurrentBedTarget = e.Bed.Target;

            _logger.LogInformation(e.Tool0.Actual.ToString());

            OnUIChanged?.Invoke();
        }

        public override void OnPageShown()
        {
            _logger.LogInformation("SHOWN");
            _octoPrintConnector.OnTemperatureChanged += OnTemperatureChangedExecute;
        }

        public override void OnPageHide()
        {
            _logger.LogInformation("HIDE");
            _octoPrintConnector.OnTemperatureChanged -= OnTemperatureChangedExecute;
        }
    }
}
