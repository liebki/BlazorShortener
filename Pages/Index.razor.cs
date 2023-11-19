using BlazorShortener.Dialogs;
using BlazorShortener.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NavigationManagerUtils;

namespace BlazorShortener.Pages
{
    partial class Index
    {
        [Inject]
        private SqliteManager SqlMan { get; set; }

        [Inject]
        private NavManUtils NavMan { get; set; }

        [Inject]
        private IDialogService DialogMan { get; set; }

        [Inject]
        private ISnackbar SnackMan { get; set; }

        private string LongUrlValue { get; set; }

        private string ShortUrlValue { get; set; }


        private async Task ButtonClickShorten()
        {
            string ServerDomainBase = NavMan.GetNavigationManager().BaseUri;

            if (!string.IsNullOrEmpty(LongUrlValue) && !string.IsNullOrWhiteSpace(LongUrlValue) && IsUrlFormatValid(LongUrlValue, ServerDomainBase))
            {
                IDialogReference dialog = DialogMan.Show<VerificationCaptchaDialog>("Verify you are a human:");
                DialogResult result = await dialog.Result;

                if (!result.Canceled)
                {
                    string ShortedUrl = await SqlMan.InsertUrl(LongUrlValue, ServerDomainBase);

                    if (IsUrlFormatValid(ShortedUrl, ServerDomainBase))
                    {
                        ShortUrlValue = ShortedUrl;
                        DialogParameters parameters = new()
                    {
                        { "ShortUrlValue", ShortUrlValue }
                    };

                        DialogMan.Show<LinkDisplayDialog>("Your link is ready:", parameters);
                        SnackMan.Add("Shorted", Severity.Success);
                    }
                    else
                    {
                        SnackMan.Add("While trying to short your url, an error occurred!", Severity.Error);
                    }
                }
                else
                {
                    SnackMan.Add("You did not pass the captcha challenge!", Severity.Error);
                }
            }
            else
            {
                LongUrlValue = string.Empty;
                SnackMan.Add("The url is not a valid url, please try again!", Severity.Warning);
            }
            LongUrlValue = string.Empty;
        }

        private static bool IsUrlFormatValid(string Url, string ServerDomainBase)
        {
            if (Url.Contains("127.0.0.1") || Url.Contains("::1") /*|| Url.Contains("localhost") || Url.Contains(ServerDomainBase)*/)
            {
                return false;
            }

            return Uri.TryCreate(Url, UriKind.Absolute, out Uri? uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

    }
}
