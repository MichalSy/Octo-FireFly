using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octo_FireFly.App.UIComponents
{
    public partial class TouchButton
    {
        [Inject]
        private IJSRuntime _runtime { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        public ElementReference _clickSoundReference;

        private async void TouchButtonExecute(MouseEventArgs e)
        {
            await _runtime.InvokeVoidAsync($"window.playTouch");
            await OnClick.InvokeAsync(e);
        }
    }
}
