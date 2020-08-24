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
  public static async Task Main(string[] args)
  {
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
   services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

   #region DI Serverkommunikation
   services.AddScoped<MiracleListAPI.MiracleListProxy>();
   services.AddScoped<AuthenticationManager>();
   #endregion
  }
 }
}