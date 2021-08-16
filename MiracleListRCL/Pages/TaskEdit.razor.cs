using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.Pages
{
 public partial class TaskEdit
 {
  [Parameter] // zu bearbeitende Aufgabe
  [EditorRequired]
  public BO.Task Task { get; set; }

  [Parameter] // Ereignis, wenn Aufgabe sich geändert hat
  public EventCallback<bool> TaskHasChanged { get; set; }

  [Inject]
  MiracleListAPI.MiracleListProxy proxy { get; set; } = null;

  [Inject] AuthenticationStateProvider asp { get; set; } = null;
  AuthenticationManager am { get { return (asp as AuthenticationManager); } }

  protected override async System.Threading.Tasks.Task OnInitializedAsync()
  {
  }

  // wenn Parameter gesetzt wird
  protected async override void OnParametersSet()
  {
  }

  protected async void Save()
  {
   await proxy.ChangeTaskAsync(this.Task, am.Token);
   await TaskHasChanged.InvokeAsync(false);
  }

  protected async void Cancel()
  {
   await TaskHasChanged.InvokeAsync(true); 
  }

 } // end class TaskEdit
}