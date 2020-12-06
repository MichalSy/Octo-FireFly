using Octo_FireFly.App.Manager.OctoPrint.Models;
using System;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Manager.OctoPrint
{
    public interface IOctoPrintConnector
    {
        EventHandler<TemperatureStatusDTO> OnTemperatureChanged { get; set; }

        Task ConnectAsync();
    }
}