using BlazorShortener.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorShortener
{
    partial class App
    {

        [Parameter]
        public string? RemoteIpAddress { get; set; }

        [Inject]
        AddressContext BlazorAppContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.BlazorAppContext.CurrentUserIP = this.RemoteIpAddress;
        }

    }
}
