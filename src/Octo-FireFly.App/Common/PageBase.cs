using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octo_FireFly.App.Common
{
    public class PageBase<T> : ComponentBase, IDisposable
        where T : ViewModelBase
    {
        private readonly ILogger<T> _logger;

        [Inject]
        public T ViewModel { get; set; }

        [Inject]
        public ILogger<PageBase<T>> Logger { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ViewModel.OnUIChanged = () => StateHasChanged();
            ViewModel.OnPageShown();
        }

        public void Dispose() => ViewModel.OnPageHide();
    }
}
