using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MiracleListAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Pages
{
 public partial class Index
 {
  [Inject]
  AuthenticationManager am { get; set; }
  [Inject]
  MiracleListAPI.MiracleListProxy proxy { get; set; }
  [Inject]
  IJSRuntime js { get; set; }

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
   // TODO: Muss sp�ter im Anmeldebildschirm erfolgen
   if (await am.Login())
   {
    await ShowCategorySet();
   }
  }

  public async Task ShowCategorySet()
  {
   categorySet = await proxy.CategorySetAsync(am.Token);
   // zeige erste Kategorie an, wenn es Kategorien gibt!
   if (this.categorySet.Count > 0) await ShowTaskSet(this.categorySet[0]);
  }

  /// <summary>
  /// Lade die Aufgaben einer Kategorie
  /// und setze die aktuelle Kategorie
  /// </summary>
  /// <param name="c">zu ladende Katagorie</param>
  /// <returns></returns>
  public async Task ShowTaskSet(BO.Category c)
  {
   this.category = c;
   this.taskSet = await proxy.TaskSetAsync(c.CategoryID, am.Token);
  }

  /// <summary>
  /// Setze aktuelle Aufgabe
  /// sp�ter: Zeige Details
  /// </summary>
  /// <param name="t"></param>
  /// <returns></returns>
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
     t.TaskID = 0; // notwendig f�r Server, da der die ID vergibt
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
    }
   }
  }

  public async Task ReloadTasks(bool reload)
  {
  
   if (reload) await ShowTaskSet(this.category); // Bei Cancel sollte die Liste der Aufgaben neu geladen werden, denn sonst geistert das modifizierte, aber nicht persistierte Objekt noch im RAM herum. 
   // Nun keine aktuelle Aufgabe mehr!
   this.task = null;
  }

  /// <summary>
  /// Ereignisbehandlung: Benutzer l�scht Aufgabe
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
  }

  /// <summary>
  /// Ereignisbehandlung: Benutzer löscht Kategorie
  /// </summary>
  /// <param name="c">zu löschende Kategorie</param>
  public async System.Threading.Tasks.Task RemoveCategory(BO.Category c)
  {
   // R�ckfrage (Browser-Dialog via JS!)
   if (!await js.InvokeAsync<bool>("confirm", "Remove Category #" + c.CategoryID + ": " + c.Name + "?")) return;
   // L�schen via WebAPI-Aufruf
   await proxy.DeleteCategoryAsync(c.CategoryID, am.Token);
   // Liste der Kategorien neu laden
   await ShowCategorySet();
   // aktuelle Category zurücksetzen
   this.category = null;
  }
 } // end class Index
}