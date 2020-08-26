using System.Threading.Tasks;
using System;
using MiracleListAPI;
using System.Security.Claims;   // NEU in Teil 3
using Microsoft.AspNetCore.Components.Authorization;   // NEU in Teil 3

namespace Web
{
 /// <summary>
 /// Authentifizierung zum Debugging
 /// </summary>
 public class AuthenticationManager : AuthenticationStateProvider // Vererbung NEU in Teil 3
 {
  MiracleListAPI.MiracleListProxy proxy { get; set; }
  Blazored.LocalStorage.ILocalStorageService localStorage { get; set; }

  public AuthenticationManager(MiracleListAPI.MiracleListProxy proxy, Blazored.LocalStorage.ILocalStorageService localStorage )
  {
   this.proxy = proxy;
   this.localStorage = localStorage;
   Console.WriteLine("Backend: " + proxy.BaseUrl);
  }

  public const string ClientID = "11111111-1111-1111-1111-111111111111";
  const string STORAGE_KEY = "TempBackendToken";
  public LoginInfo CurrentLoginInfo = null;
  public string Token { get { return CurrentLoginInfo?.Token; } }

  // NEU in Teil 4
  public async Task<bool> CheckLocalTokenValid()
  {
   bool result = false;

   string token = await localStorage.GetItemAsync<string>(STORAGE_KEY);
   if (!String.IsNullOrEmpty(token))
   {
    // Es gibt ein Token im Local Storage. Nachfrage beim Server, ob noch gültig.
    Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(CheckLocalTokenValid)}: Checking local token {token}...");
    var l = new LoginInfo() {Token=token, ClientID = AuthenticationManager.ClientID };
    this.CurrentLoginInfo = await proxy.LoginAsync(l);
    if (this.CurrentLoginInfo == null || String.IsNullOrEmpty(CurrentLoginInfo.Token))
    { // Token ungültig!
     Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(CheckLocalTokenValid)}: Token not valid: {CurrentLoginInfo?.Message}!");
     CurrentLoginInfo = null;
    }
    else
    { // Token gültig!
     Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(CheckLocalTokenValid)}: Found valid Token: {CurrentLoginInfo.Token} for User: {CurrentLoginInfo.Username}");
     Notify();
     result = true;
    }
   }
   else
   {
    Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(CheckLocalTokenValid)}: No local token!");
   }
   Notify();
   return result;
  }

  /// <summary>
  /// Login to be called by Razor Component Login.razor
  /// </summary>
  public async Task<bool> Login(string username, string password)
  {
   bool result = false;
   CurrentLoginInfo = null;
   var l = new LoginInfo() { Username = username, Password = password, ClientID = AuthenticationManager.ClientID };
   try
   {
    CurrentLoginInfo = await proxy.LoginAsync(l);
    if (String.IsNullOrEmpty(CurrentLoginInfo.Token))
    {
     Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(Login)}: Anmeldung NICHT erfolgreich: " + username);
    }
    else
    {
     result = true;
     await localStorage.SetItemAsync<string>(STORAGE_KEY, this.CurrentLoginInfo.Token);
     Console.WriteLine($"{nameof(AuthenticationManager)}.{ nameof(Login)}: Anmeldung erfolgreich: " + this.CurrentLoginInfo.Username);
    }
   }
   catch (Exception ex)
   {
    Console.WriteLine($"{nameof(AuthenticationManager)}.{ nameof(Login)}: Anmeldefehler: " + ex.Message);
   }
   Notify();
   return result;
  }

  /// <summary>
  /// Logout to be called by Razor Component Login.razor
  /// </summary>
  public async Task Logout()
  {
   if (this.CurrentLoginInfo == null) return;
   var e = await proxy.LogoffAsync(this.CurrentLoginInfo.Token);
   if (e)
   {
    // Remove LoginInfo in RAM for clearing authenticaton state
    CurrentLoginInfo = null;
    // Remove LoginInfo in browser local storage
    await localStorage.RemoveItemAsync(STORAGE_KEY);
    Notify();
    Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(Logout)}: Logout OK!");
   }
   else
   {
    Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(Logout)}: Logout Error!");
   }
  }

  /// <summary>
  /// NEU in Teil 3: Notify Blazor infrastructure about new Authentication State
  /// </summary>
  private void Notify()
  {
   this.NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
  }

  // NEU in Teil 3
  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
   if (this.CurrentLoginInfo != null && !String.IsNullOrEmpty(this.CurrentLoginInfo.Token) && !String.IsNullOrEmpty(proxy.BaseUrl))
   {
    const string authType = "MiracleList WebAPI Authentication";
    var identity = new ClaimsIdentity(new[]
    {
    new Claim("Backend", proxy.BaseUrl),
    new Claim(ClaimTypes.Sid, this.CurrentLoginInfo.Token), // use SID claim for token
    new Claim(ClaimTypes.Name, this.CurrentLoginInfo.Username),
    }, authType);

    var cp = new ClaimsPrincipal(identity);
    var state = new AuthenticationState(cp);
    Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(GetAuthenticationStateAsync)}: {this.CurrentLoginInfo.Username}");
    return state;
   }
   else
   {
    Console.WriteLine($"{nameof(AuthenticationManager)}.{nameof(GetAuthenticationStateAsync)}: No user!");
    var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    return state;
   }
  }
 }
}