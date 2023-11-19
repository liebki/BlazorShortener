using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorShortener.Dialogs
{
    partial class LinkDisplayDialog
    {
        [Parameter] public string ShortUrlValue { get; set; }

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));

    }
}
