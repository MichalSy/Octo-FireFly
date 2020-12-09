using Octo_FireFly.App.Manager.OctoPrint.Models;
using System;

namespace Octo_FireFly.App.Manager.Temperature
{
    public interface ITemperatureManager
    {
        EventHandler OnTemperatureChanged { get; set; }
        decimal CurrentToolActual { get; set; }
        decimal CurrentToolTarget { get; set; }
        decimal CurrentBedActual { get; set; }
        decimal CurrentBedTarget { get; set; }
    }
}