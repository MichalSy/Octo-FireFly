using Microsoft.AspNetCore.Components;
using Octo_FireFly.App.Pages.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Common
{
    public abstract class ViewModelBase
    {
        public Action OnUIChanged { get; set; }

        public virtual void OnPageShown() { }

        public virtual void OnPageHide() { }
    }
}
