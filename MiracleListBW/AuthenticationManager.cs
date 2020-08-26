﻿using System.Threading.Tasks;
using System;
using System.Security.Claims;   // NEU in Teil 3
using Microsoft.AspNetCore.Components.Authorization;   // NEU in Teil 3
using MiracleListAPI;
using Microsoft.Extensions.Primitives;

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
  public LoginInfo CurrentLoginInfo = null;
  const string STORAG_KEY = "TempBackendToken";

  public string Token { get { return CurrentLoginInfo?.Token; } }

  public async Task<bool> CheckLocalTokenValid()
  {
   bool result = false;
   string token = await localStorage.GetItemAsync<string>(STORAG_KEY);
   if (!String.IsNullOrEmpty(token))
   {
    // Es gibt ein Token im Local Storage. Nachfrage beim Server, ob noch gültig.
    Console.WriteLine("Checking local token " + token  + "...");
    var l = new LoginInfo() {Token=token, ClientID = AuthenticationManager.ClientID };
    this.CurrentLoginInfo = await proxy.LoginAsync(l);
    if (this.CurrentLoginInfo == null || String.IsNullOrEmpty(CurrentLoginInfo.Token))
    { // Token ungültig!
     Console.WriteLine($"{nameof(CheckLocalTokenValid)}: Token not valid: {CurrentLoginInfo?.Message}!");
     CurrentLoginInfo = null;
    }
    else
    { // Token gültig!
     Console.WriteLine($"{nameof(CheckLocalTokenValid)}: Found valid Token = : {CurrentLoginInfo.Token}");
     Notify();
     result = true;
    }
   }
   else
   {
    Console.WriteLine($"{nameof(CheckLocalTokenValid)}: No local token!");
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
     Console.WriteLine("Anmeldung NICHT erfolgreich: " + this.CurrentLoginInfo.Username);
    }
    else
    {
     result = true;
     await localStorage.SetItemAsync<string>(STORAG_KEY, this.CurrentLoginInfo.Token);
     Console.WriteLine("Anmeldung erfolgreich: " + this.CurrentLoginInfo.Username);
    }
   }
   catch (Exception ex)
   {
    Console.WriteLine("Anmeldefehler: " + ex.Message);
   }
   Notify();
   return result;
  }

  /// <summary>
  /// Logout to be called by Razor Component Login.razor
  /// </summary>
  public async Task Logout()
  {
   Console.WriteLine("Logout", this.CurrentLoginInfo);
   if (this.CurrentLoginInfo == null) return;
   var e = await proxy.LogoffAsync(this.CurrentLoginInfo.Token);
   if (e)
   {
    // Remove LoginInfo in RAM for clearing authenticaton state
    CurrentLoginInfo = null;
    Notify();
   }
   else
   {
    Console.WriteLine("Logout Error!");
   }
  }

  /// <summary>
  /// NEU in Teil 3: Notify Blazor infrastructure about new Authentication State
  /// </summary>
  private void Notify()
  {
   Console.WriteLine("Notify: " + CurrentLoginInfo?.Username);
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
    Console.WriteLine("GetAuthenticationStateAsync: " + this.CurrentLoginInfo.Username);
    return state;
   }
   else
   {
    Console.WriteLine("GetAuthenticationStateAsync: no user");
    var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    return state;
   }
  }
 }
}