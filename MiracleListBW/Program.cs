
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Web
{
 public class Program
 {
  // DEMO: Startcode
  public static async Task Main(string[] args)
  {
   // DEMO: Startcode
   Console.WriteLine("Blazor WebAssembly Main()");
   Console.WriteLine(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
   Console.WriteLine("App-Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

   var builder = WebAssemblyHostBuilder.CreateDefault(args);
   builder.RootComponents.Add<App>("app");
   AddServices(builder);

   Console.WriteLine("Environment: " + builder.HostEnvironment.Environment);
   Console.WriteLine("BaseAddress: " + builder.HostEnvironment.BaseAddress);
   Console.WriteLine("RootComponent: " + builder.RootComponents[0].Selector);

   Console.WriteLine("Starting...");
   await builder.Build().RunAsync();
  }

  public static void AddServices(WebAssemblyHostBuilder builder)
  {
   IServiceCollection services = builder.Services;
   // Neu seit 3.2 Preview 2
   //services.AddBaseAddressHttpClient();

   // neu seit 3.2 Preview 4:
   services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

   #region DI Authentifizierung
   //services.AddOptions(); // notwendig für AuthenticationStateProvider
   //services.AddScoped<AuthenticationStateProvider, MLAuthenticationStateProvider>();
   //services.AddAuthorizationCore(); // sonst: System.InvalidOperationException: Cannot provide a value for property 'AuthorizationPolicyProvider' on type 'Microsoft.AspNetCore.Components.Authorization.AuthorizeView'. There is no registered service of type 'Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider'.
   #endregion

   #region DI Serverkommunikation
   services.AddSingleton<MiracleListAPI.MiracleListProxy>();
   //services.AddSingleton<AuthenticationManager>();
   services.AddSingleton<AuthenticationStateProvider, AuthenticationManager>();
   services.AddAuthorizationCore();
   #endregion

   #region DI JavaScript-Interoperabilität
   //services.AddScoped<BlazorUtil>();
   #endregion

   #region DI State & Storage
   //Für Session state
   //services.AddScoped<MLBlazorRCL.Komponentenzustand.TypedSessionState>();
   //services.AddScoped<MLBlazorRCL.Komponentenzustand.GenericSessionState>();

   // NUGET: Blazored.LocalStorage;
   // GITHUB:  https://github.com/Blazored/LocalStorage --> 
   // Startup: using Blazored.LocalStorage / services.AddBlazoredLocalStorage();
   //services.AddBlazoredLocalStorage();

   //// braucht BlazirUtil
   //services.AddScoped<IHttpContextAccessor, WebAssemblyHttpContextAccessorDummy>();

   // Alternativ möglich (einige API-Änderungen!)
   // NUGET: Blazor.Extensions.Storage
   // GITHUB: https://github.com/BlazorExtensions/Storage
   // using Blazor.Extensions.Storage;
   //services.AddBlazoredLocalStorage();

   // BISHER NICHT MÖGLICH: Protected Browser Storage
   // services.AddProtectedBrowserStorage();
   #endregion

   #region Logging

   // 3.1 isn't supported yet. https://github.com/BlazorExtensions/Logging/issues/41
   //services.AddLogging(builder => builder
   // .AddBrowserConsole() // Add Blazor.Extensions.Logging.BrowserConsoleLogger
   // .SetMinimumLevel(LogLevel.Trace)
   //);
   #endregion
  }
 }
}