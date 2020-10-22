export function confirmWithLog(text, log = false) {
 if (log) console.log("confirm: " + text);
 var e = confirm(text);
 if (log) console.log("confirm: " + text + "=" + e);
 return e;
}


// Modaler Bestätigungsdialog mit Bootstrap
export function confirmBootstrap(dotnetObj, taskID, text, log = false) {
 if (log) console.log(confirmBootstrap, dotnetObj, taskID, text);
 // Setzte Text
 $("#confirmModalText").html(text);
 // Binde Ereignis für Schaltfläche 1 ("Yes")
 $("#confirmModalText-btn-yes").on("click", function () {
  if (log) console.log("#confirmModalText-btn-si", taskID);
  $("#confirmModal").modal('hide');
  dotnetObj.invokeMethodAsync("ConfirmedRemoveTask", taskID, true);
  $("#confirmModalText-btn-yes").off();
 });
 // Binde Ereignis für Schaltfläche 2 ("No")
 $("#confirmModalText-btn-no").on("click", function () {
  if (log) console.log("#confirmModalText-btn-no", taskID);
  $("#confirmModal").modal('hide');
  dotnetObj.invokeMethodAsync("ConfirmedRemoveTask", taskID, false);
  $("#confirmModalText-btn-no").off();
 });
 // Zeige Dialog
 $("#confirmModal").modal();
}


console.log("MiracleListRCL.js geladen!");