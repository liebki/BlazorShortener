using BlazorVerificationCaptcha;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorShortener.Dialogs
{
    partial class VerificationCaptchaDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        private string VerificationValue { get; set; }

        private string VerificationCheckValue { get; set; } = string.Empty;

        private void Submit()
        {
            if (VerificationCheckValue.Equals(VerificationValue, StringComparison.InvariantCulture))
            {
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                MudDialog.Close(DialogResult.Cancel());
            }
        }

        protected override Task OnInitializedAsync()
        {
            VerificationValue = VerificationCaptcha.GenerateCaptchaContent();
            return base.OnInitializedAsync();
        }

    }
}
