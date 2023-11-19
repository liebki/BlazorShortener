using BlazorShortener.Services;
using MudBlazor;
using MudBlazor.Services;
using NavigationManagerUtils;
using System.Net;

namespace BlazorShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLettuceEncrypt();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                config.SnackbarConfiguration.PreventDuplicates = true;
                config.SnackbarConfiguration.NewestOnTop = true;
                config.SnackbarConfiguration.ShowCloseIcon = false;
                config.SnackbarConfiguration.VisibleStateDuration = 5000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            builder.Services.AddScoped<SqliteManager>();
            builder.Services.AddTransient<NavManUtils>();

            builder.Services.AddScoped<AddressContext>();
            builder.Services.AddHttpContextAccessor();

            builder.WebHost.UseKestrel(k =>
            {
                IServiceProvider appServices = k.ApplicationServices;
                k.Listen(
                    IPAddress.Any, 443,
                    o => o.UseHttps(h =>
                    {
                        h.UseLettuceEncrypt(appServices);
                    }));
            });

            WebApplication app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapBlazorHub();

            app.MapFallbackToPage("/_Host");
            app.Run();
        }
    }
}