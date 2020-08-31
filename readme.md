# Blazor WebAssembly Tutorial für Heise Developer

Schrittweiser Einstieg in [Blazor WebAssembly](https://dotnet-lexikon.de/Blazor_WebAssembly/lex/9768.aspx) anhand der [Single-Page-Webanwendung "MiracleList"](http://miraclelist-bw.azurewebsites.net/). Dieses Tuorial wird im Herbst 2020 bei [www.heise.de/developer](https://www.heise.de/developer) erscheinen (Termine noch offen).

![MiracleList Logo](https://raw.githubusercontent.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/Master/MiracleListBW/wwwroot/img/MiracleListLogo.jpg "MiracleList Logo")

# Anleitung

Sobald die einzelnen Teile des Tutorials erschienen sind, werden diese hier verlinkt:
- Teil 1: Web API-Aufrufe und Rendern von Daten (ca. 18.000 Zeichen)
- Teil 2: Eingabesteuerelemente und JavaScript-Interoperabilität (ca. 20.000 Zeichen)
- Teil 3: Authentifizierung und Autorisierung (ca. 20.000 Zeichen)
- Teil 4: Zustandsverwaltung und Nachladen von Modulen (ca. 19.000 Zeichen)
- Teil 5: Bi-Direktionale Kommunikation und Benachrichtigungen (ca. 19.000 Zeichen)

# Voraussetzungen
- [Visual Studio 2019 v16.8](https://visualstudio.microsoft.com/de/vs/preview/) oder höher (Alternativ: v16.7, damit ist dann aber nur Blazor WebAssembly 3.2 nutzbar!)
- [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) Preview 8 oder höher (Alternativ: Blazor WebAssembly 3.2 im [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1), Lazy Loading in Teil 4 damit nicht lösbar!)
- Wichtig: Sie benötigen eine eigene [Client-ID für das cloudbasierte MiracleList-Backend](http://miraclelistbackend.azurewebsites.net/clientid)!

# Inkompabitilitäten
- Lazy Loading (Teil 4) ist erst ab Blazor 5.0 möglich
- ASP.NET SignalR (Teil 5) funktioniert nicht mit Blazor 5.0 Preview 8 :-( --> verwenden Sie hier MiracleListBW32.csproj!

# Quellcodestände

Die Quellcodestände vor/nach den einzelnen Tutorial-Teilen finden Sie in den Branches
- [Ausgangszustand / Beginn Teil 1][0]
- [Stand nach Teil 1 / Beginn Teil 2][1]
- [Stand nach Teil 2 / Beginn Teil 3][2]
- [Stand nach Teil 3 / Beginn Teil 4][3]
- [Stand nach Teil 4 / Beginn Teil 5][4]
- [Stand nach Teil 5][5]

[0]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/Ausgangszustand
[1]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil1
[2]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil2
[3]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil3
[4]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil4
[5]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil5



# Webadressen

- [Blazor WebAssembly](https://dotnet-lexikon.de/Blazor_WebAssembly/lex/9768.aspx) im .NET-Lexikon 
- [Fertige Lösung "MiracleList"](http://miraclelist-bw.azurewebsites.net/) auf Basis Blazor WebAssembly Single-Page-Webanwendung "MiracleList"
- [Cloudbasiertes MiracleList-Backend](http://miraclelistbackend.azurewebsites.net/clientid) basiert auf .NET Core 3.1  --> [Quellcode auf GitHub](https://github.com/HSchwichtenberg/MiracleListBackend)
