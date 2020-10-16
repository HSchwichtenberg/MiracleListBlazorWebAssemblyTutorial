using MiracleListAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
 /// <summary>
 /// Authentifizierung zum Debugging
 /// </summary>
 public class AuthenticationManager
 {
  MiracleListAPI.MiracleListProxy proxy { get; set; }

  public AuthenticationManager(MiracleListAPI.MiracleListProxy proxy)
  {
   this.proxy = proxy;
  }

  public const string DebugUser = "iXTutorial";
  public const string DebugPassword = "Sehr+Geheim"; // :-)
  public const string ClientID = "11111111-1111-1111-1111-111111111111"; // TODO: Hier Ihre persönliche Client-ID eintragen! http://miraclelistbackend.azurewebsites.net/clientid
  public LoginInfo CurrentLoginInfo { get; set; } = null;
  public string Token { get { return CurrentLoginInfo?.Token; } }

  public async Task<bool> Login()
  {
   var l = new LoginInfo() { Username = AuthenticationManager.DebugUser, Password = AuthenticationManager.DebugPassword, ClientID = AuthenticationManager.ClientID };
   try
   {
    CurrentLoginInfo = await proxy.LoginAsync(l);
    if (String.IsNullOrEmpty(CurrentLoginInfo.Token))
    {
     Console.WriteLine("Anmeldung NICHT erfolgreich: " + this.CurrentLoginInfo.Username);
     return false;
    }
    else
    {
     Console.WriteLine("Anmeldung erfolgreich: " + this.CurrentLoginInfo.Username);
     return true;
    }
   }
   catch (Exception ex)
   {
    Console.WriteLine("Anmeldefehler: " + ex.Message);
    return false;
   }
  }
 }
}