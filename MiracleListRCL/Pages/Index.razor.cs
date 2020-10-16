using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MiracleListAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Web.Pages
{
 public partial class Index
 {
  [Inject] AuthenticationStateProvider asp { get; set; } = null;
  AuthenticationManager am { get { return (asp as AuthenticationManager); } }
  [Inject]
  MiracleListAPI.MiracleListProxy proxy { get; set; }
  [Inject]
  IJSRuntime js { get; set; }
  [Inject] public NavigationManager NavigationManager { get; set; }
  [CascadingParameter] 
  Task<AuthenticationState> authenticationStateTask { get; set; }
  [Inject] Blazored.Toast.Services.IToastService toastService { get; set; }

  #region Infrastruktur-Objekte
  HubConnection hubConnection;
  ClaimsPrincipal user;
  #endregion

  #region Einfache Properties zur Datenbindung
  List<BO.Category> categorySet { get; set; }
  List<BO.Task> taskSet { get; set; }
  BO.Category category { get; set; }
  BO.Task task { get; set; }
  string newCategoryName { get; set; }
  string newTaskTitle { get; set; }

  #endregion

  /// <summary>
  /// Lebenszyklusereignis: Komponente wird initialisiert
  /// </summary>
  /// <returns></returns>
  protected override async Task OnInitializedAsync()
  {
   Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
   user = (await authenticationStateTask).User;
   //if (!user.Identity.IsAuthenticated) { this.NavigationManager.NavigateTo("/"); return; }
   await ShowCategorySet();

   var hubURL = new Uri(new Uri(proxy.BaseUrl), "MLHub");
   Console.WriteLine("SignalR: Connect to " + hubURL.ToString());
   hubConnection = new HubConnectionBuilder()
       .WithUrl(hubURL)
       .Build();

   // --- eingehende Nachricht
   hubConnection.On<string>("CategoryListUpdate", async (connectionID) =>
   {
    if (hubConnection.ConnectionId != connectionID)
    {
     string s = $"Kategorieliste wurde auf einem anderen System geändert.";
     Console.WriteLine(s);
     toastService.ShowSuccess(s, "Kategorien geändert");
     await ShowCategorySet();
     StateHasChanged();
    }
   });
   // --- eingehende Nachricht
   hubConnection.On<string, int>("TaskListUpdate", async (connectionID, categoryID) =>
   {
   if (hubConnection.ConnectionId != connectionID)
   {
    string s = $"Aufgaben der Kategorie #{category.CategoryID}: \"{this.category.Name}\" wurden auf einem anderen System geändert.";
    Console.WriteLine(s);
    toastService.ShowInfo(s, "Aufgaben geändert");
    if (categoryID == this.category.CategoryID) await ShowTaskSet(this.category);
    StateHasChanged();
    }
   });

   // Verbindung zum SignalR-Hub starten
   await hubConnection.StartAsync();
   // Registrieren für Events
   await hubConnection.SendAsync("Register", user.Identity.Name);
  }

  public async Task ShowCategorySet()
  {
   categorySet = await proxy.CategorySetAsync(am.Token);
   // zeige erste Kategorie an, wenn es Kategorien gibt!
   if (this.categorySet.Count > 0) await ShowTaskSet(this.categorySet[0]);
  }

   public async Task ShowTaskSet(BO.Category c)
  {
   this.category = c;
   this.taskSet = await proxy.TaskSetAsync(c.CategoryID, am.Token);
  }

  public async Task ShowTaskDetail(BO.Task t)
  {
   this.task = t;
  }

  /// <summary>
  /// Use Keyup instead of Keypress as the actual data binding did not yet happen when Keypress is fired
  /// </summary>
  public async Task NewCategory_Keyup(KeyboardEventArgs e)
  {
   if (e.Key == "Enter")
   {
    if (!String.IsNullOrEmpty(this.newCategoryName))
    {
     var newcategory = await proxy.CreateCategoryAsync(newCategoryName, am.Token);
     await ShowCategorySet();
     await ShowTaskSet(newcategory);
     this.newCategoryName = "";
     // SignalR-Nachricht senden
     await hubConnection.SendAsync("SendCategoryListUpdate", user.Identity.Name);
    }
   }
  }

  /// <summary>
  /// Use Keyup instead of Keypress as the actual data binding did not yet happen when Keypress is fired
  /// </summary>
  public async Task NewTask_Keyup(KeyboardEventArgs e)
  {
   if (e.Key == "Enter")
   {
    if (!String.IsNullOrEmpty(this.newTaskTitle))
    {
     if (string.IsNullOrEmpty(newTaskTitle)) return;
     var t = new BO.Task();
     t.TaskID = 0; // notwendig für Server, da der die ID vergibt
     t.Title = newTaskTitle;
     t.CategoryID = this.category.CategoryID;
     t.Importance = BO.Importance.B;
     t.Created = DateTime.Now;
     t.Due = null;
     t.Order = 0;
     t.Note = "";
     t.Done = false;
     await proxy.CreateTaskAsync(t, am.Token);
     await ShowTaskSet(this.category);
     this.newTaskTitle = "";
     // SignalR-Nachricht senden
     await hubConnection.SendAsync("SendTaskListUpdate", user.Identity.Name, this.category.CategoryID);
    }
   }
  }


  public async Task ReloadTasks(int taskID)
  {
   // reload all tasks in current category
   //await ShowTaskSet(this.category);
   this.task = null;
   // SignalR-Nachricht senden
   await hubConnection.SendAsync("SendTaskListUpdate", user.Identity.Name, this.category.CategoryID);
  }

  /// <summary>
  /// Ereignisbehandlung: Benutzer löscht Aufgabe
  /// </summary>
  public async System.Threading.Tasks.Task RemoveTask(BO.Task t)
  {
   // Rückfrage (Browser-Dialog via JS!)
   if (!await js.InvokeAsync<bool>("confirm", "Remove Task #" + t.TaskID + ": " + t.Title + "?")) return;
   // Löschen via WebAPI-Aufruf
   await proxy.DeleteTaskAsync(t.TaskID, am.Token);
   // Liste der Aufgaben neu laden
   await ShowTaskSet(this.category);
   // aktuelle Aufgabe zurücksetzen
   this.task = null;
   // SignalR-Nachricht senden
   await hubConnection.SendAsync("SendTaskListUpdate", user.Identity.Name, this.category.CategoryID);
  }

  /// <summary>
  /// Ereignisbehandlung: Benutzer löscht Kategorie
  /// </summary>
  /// <param name="c">zu löschende Kategorie</param>
  public async System.Threading.Tasks.Task RemoveCategory(BO.Category c)
  {
   // Rückfrage (Browser-Dialog via JS!)
   if (!await js.InvokeAsync<bool>("confirm", "Remove Category #" + c.CategoryID + ": " + c.Name + "?")) return;
   // Löschen via WebAPI-Aufruf
   await proxy.DeleteCategoryAsync(c.CategoryID, am.Token);
   // Liste der Kategorien neu laden
   await ShowCategorySet();
   // aktuelle Category zurücksetzen
   this.category = null;
   // SignalR-Nachricht senden
   await hubConnection.SendAsync("SendCategoryListUpdate", user.Identity.Name);
  }
 } // end class Index
}