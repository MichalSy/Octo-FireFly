using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Server.Manager.AutoUpdate
{
    public interface IAutoUpdater
    {
        Task CheckForUpdateAsync();
    }
}
