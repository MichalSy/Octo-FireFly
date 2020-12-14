using System;

namespace Octo_FireFly.App.Common
{
    public abstract class ViewModelBase
    {
        public Action OnUIChanged { get; set; }

        public virtual void OnPageShown() { }

        public virtual void OnPageHide() { }
    }
}
