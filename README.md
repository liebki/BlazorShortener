# BlazorShortener

BlazorShortener is a simple and small but complete URL shortener application, created in C# with Blazor Server .NET 7.0, and several essential NuGet packages. This is a simple URL shortening solution with features like captcha verification, Let's Encrypt integration, SQLite database for link management, and a (in my opinion) visually appealing interface.

## Project Dependencies

- [BlazorVerificationCaptcha](https://github.com/liebki/BlazorVerificationCaptcha)
- [LettuceEncrypt](https://github.com/natemcmaster/LettuceEncrypt)
- [Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/)
- [NavigationManagerUtils](https://github.com/liebki/NavigationManagerUtils)
- [MudBlazor](https://mudblazor.com/)
- [SkiaSharp](https://www.nuget.org/packages/SkiaSharp/)

## Project Overview

- **Captcha Integration**: BlazorShortener uses the [BlazorVerificationCaptcha](https://github.com/liebki/BlazorVerificationCaptcha) library to implement an easy-to-use captcha solution generated on the server. If running on a Linux server, ensure all SkiaSharp dependencies are installed.

- **Secure HTTPS Connections**: Integration with [LettuceEncrypt](https://github.com/natemcmaster/LettuceEncrypt) allows for seamless domain integration with Let's Encrypt. Simply update the `appsettings.json` file with the required information.

- **SQLite Database**: BlazorShortener utilizes a small SQLite database named "BlazorShortener.db" with tables for managing short and long URLs, as well as tracking user access (be sure to include this in the privacy policy!)

- **Homepage Design**: The homepage features buttons linking to your GitHub profile, the homepage, and a privacy policy template. The dynamic background is fetched from Unsplash's collection (ID: 772336) on each page load.

- **MudBlazor Styling**: The project uses [MudBlazor](https://mudblazor.com/) for styling components. Notably, toast messages provide user feedback on actions like successful URL shortening, captcha errors, etc.


## Installation and Usage

1. **If you dont't want to use LettuceEncrypt:**
   - You need to comment out following lines in the ```program.cs```:
   -  ```builder.Services.AddLettuceEncrypt();```
   -
   ```
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
   ```
   - or at least the ```h.UseLettuceEncrypt(appServices);``` inside ```builder.WebHost.UseKestrel()```

2. **Install .NET 7 Runtime (Including ASP.NET):**
   - Ensure that you have the .NET 7 runtime installed, including ASP.NET support. You can download it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/7.0).

3. **Publish the Project:**
   - Use the [dotnet publish](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish) command to publish the project. For example:
     ```dotnet publish -c Release```
   - Note: If you're using Visual Studio, you can also use its graphical interface to export projects. 
   
4. **Configure Appsettings.json:**
   - Make sure to configure the `appsettings.json` file with the required information for BlazorShortener, such as Let's Encrypt integration details and any other relevant settings.

5. **Run the Assembly:**
   - After publishing, navigate to the published directory.
   - On Linux, you'll find a .dll file; on Windows, it will be a .exe file. (Depending on the command you used tho)
   - Run the assembly using the dotnet command line tools (Linux):
     ```dotnet BlazorShortener.dll```
   - Or just run the .exe file on Windows (command line is still an option tho).
   - Ensure to check if everything works as expected.

6. **Access the Application:**
   - Open your web browser and navigate to the appropriate URL where the application is hosted (e.g., http://localhost:5000 or the domain you are using).

By following these steps, you should have BlazorShortener up and running. If you encounter any issues during installation or have specific configuration requirements, refer to the project documentation or seek assistance in the [GitHub repository issues](https://github.com/liebki/BlazorShortener/issues).

## Screenshots / Videos

![Homepage](https://i.imgur.com/mzSzOZd.png)

## License

**Software:** BlazorShortener

License: [GNU General Public License v3.0](https://choosealicense.com/licenses/gpl-3.0/)