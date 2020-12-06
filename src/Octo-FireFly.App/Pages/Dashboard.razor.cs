using Microsoft.AspNetCore.Components;
using Octo_FireFly.App.Manager.OctoPrint;
using Octo_FireFly.App.Manager.OctoPrint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Pages
{
    public partial class Dashboard
    { 
        [Inject]
        public IOctoPrintConnector OctoPrintConnector { get; set; }

        public decimal CurrentToolActual { get; set; }
        public decimal CurrentToolTarget { get; set; }

        public decimal CurrentBedActual { get; set; }
        public decimal CurrentBedTarget { get; set; }


        protected override void OnInitialized()
        {
            OctoPrintConnector.OnTemperatureChanged += OnTemperatureChangedExecute;
        }

        private void OnTemperatureChangedExecute(object sender, TemperatureStatusDTO e)
        {
            CurrentToolActual = e.Tool0.Actual;
            CurrentToolTarget = e.Tool0.Target;
            CurrentBedActual = e.Bed.Actual;
            CurrentBedTarget = e.Bed.Target;

            StateHasChanged();
        }
    }
}
