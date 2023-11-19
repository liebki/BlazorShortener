using BlazorShortener.Services;
using Microsoft.AspNetCore.Components;
using NavigationManagerUtils;

namespace BlazorShortener.Pages
{
    partial class Redirect
    {
        [Inject]
        AddressContext BlazorAppContext { get; set; }

        [Inject]
        public SqliteManager SqlMan { get; set; }

        [Inject]
        public NavManUtils NavMan { get; set; }

        [Parameter]
        public string ShorturlValue { get; set; }

        private string RedirectUrlValue { get; set; }

        protected override async Task OnInitializedAsync()
        {
            RedirectUrlValue = SqlMan.GetLongurl(ShorturlValue);

            if (string.IsNullOrEmpty(RedirectUrlValue))
            {
                RedirectUrlValue = "/";
            }
            else
            {
                SqlMan.ShortUrlAccessed(ShorturlValue, BlazorAppContext.CurrentUserIP);
            }

            NavMan.Navigate(RedirectUrlValue);
        }
    }
}
