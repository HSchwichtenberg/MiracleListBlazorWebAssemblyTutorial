using Microsoft.AspNetCore.Components;
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

  #region Einfache Properties zur Datenbindung
  List<BO.Category> categorySet { get; set; }
  List<BO.Task> taskSet { get; set; }
  BO.Category category { get; set; }
  BO.Task task { get; set; }
  #endregion

  /// <summary>
  /// Lebenszyklusereignis: Komponente wird initialisiert
  /// </summary>
  /// <returns></returns>
  protected override async Task OnInitializedAsync()
  {
   if (await am.Login())
   {
    categorySet = await proxy.CategorySetAsync(am.CurrentLoginInfo.Token);
    // zeige erste Kategorie an, wenn es Kategorien gibt!
    if (this.categorySet.Count > 0) await ShowTaskSet(this.categorySet[0]);
   }
  }

  public async Task ShowTaskSet(BO.Category c)
  {
   this.category = c;
   this.taskSet = await proxy.TaskSetAsync(c.CategoryID, am.CurrentLoginInfo.Token);
  }

  public async Task ShowTaskDetail(BO.Task t)
  {
   this.task = t;
  }

 } // end class Index
}