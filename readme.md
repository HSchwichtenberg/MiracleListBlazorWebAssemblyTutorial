# Blazor WebAssembly Tutorial für Heise Developer

Schrittweiser Einstieg in [Blazor WebAssembly](https://dotnet-lexikon.de/Blazor_WebAssembly/lex/9768.aspx) anhand des praxisnahen Fallbeispiels [Single-Page-Webanwendung "MiracleList"](https://www.miraclelist.net/). 

![MiracleList Logo](https://raw.githubusercontent.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/Master/MiracleListBW/wwwroot/img/MiracleListLogo.jpg "MiracleList Logo")

# Status der Beiträge
- Das Tutorial wurde am 3.9.2020 komplett (alle fünf Teile) bei der Heise Redaktion abgeliefert.
- Die Redaktion hat den Text in den Folgemonaten sukzessive veröffentlicht. Das Tutorial ist zwischen Oktober 2020 und März 2021 bei [www.heise.de/developer](https://www.heise.de/developer) erschienen.
- Der Code wurde inzwischen auf .NET 7.0 aktualisiert. Der Text im Tutorial auf Heise.de kann aber leider nicht mehr geändert werden :-(

# Webadressen zu den Beiträgen
Sobald die einzelnen Teile des Tutorials erschienen sind, werden diese hier verlinkt:
- [Einleitungsbeitrag: Blazor Server und Blazor WebAssembly: Alternativen zu JavaScript?](https://www.heise.de/hintergrund/Blazor-Server-und-Blazor-WebAssembly-Alternativen-zu-JavaScript-4907799.html) - erschienen am 22.9.2020
- [Teil 1: Web API-Aufrufe und Rendern von Daten (ca. 18.000 Zeichen)](https://www.heise.de/ratgeber/Webprogrammierung-mit-Blazor-WebAssembly-Teil-1-Web-API-Aufrufe-und-Rendering-4932237.html) - erschienen am 23.10.2020
- [Teil 2: Eingabesteuerelemente und JavaScript-Interoperabilität (ca. 20.000 Zeichen)](https://www.heise.de/ratgeber/Blazor-WebAssembly-Teil-2-Eingabesteuerelemente-JavaScript-Interoperabilitaet-4971874.html) - erschienen am 27.11.2000
- [Teil 3: Authentifizierung und Autorisierung  (ca. 20.000 Zeichen)](https://www.heise.de/ratgeber/Blazor-WebAssembly-Teil-3-Authentifizierung-und-Autorisierung-4988529.html) - erschienen am 30.12.2020
- [Teil 4: Zustandsverwaltung und Nachladen von Modulen (ca. 19.000 Zeichen)](https://www.heise.de/ratgeber/Blazor-WebAssembly-Teil-4-Zustandsverwaltung-und-Nachladen-von-Modulen-5036983.html) - erschienen am 29.01.2021
- [Teil 5: Bi-Direktionale Kommunikation und Benachrichtigungen (ca. 19.000 Zeichen)](https://www.heise.de/ratgeber/Blazor-WebAssembly-Bidirektionale-Kommunikation-und-Benachrichtigungen-5069045.html) - erschienen am 02.03.2021

# Voraussetzungen
- [Visual Studio 2022 ab Version 17.4](https://visualstudio.microsoft.com/de/vs/preview/)
- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) 
- Wichtig: Sie benötigen eine eigene [Client-ID für das cloudbasierte MiracleList-Backend](http://miraclelistbackend.azurewebsites.net/clientid)!

# Quellcodestände
Die Quellcodestände vor/nach den einzelnen Tutorial-Teilen finden Sie in den Branches
- v0.0.x [Ausgangszustand / Beginn Teil 1][0]
- v0.1.x [Stand nach Teil 1 / Beginn Teil 2][1]
- v0.2.x [Stand nach Teil 2 / Beginn Teil 3][2]
- v0.3.x [Stand nach Teil 3 / Beginn Teil 4][3]
- v0.4.x [Stand nach Teil 4 / Beginn Teil 5][4]
- v0.5.x [Stand nach Teil 5][5]

[0]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/Ausgangszustand
[1]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil1
[2]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil2
[3]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil3
[4]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil4
[5]: https://github.com/HSchwichtenberg/MiracleListBlazorWebAssemblyTutorial/tree/EndeTeil5

Hinweis: Der Master-Branch enthält gegenüber bei Heise veröffentlichten Tutorial folgende weitere Funktionen
- Nachladen von JavaScript-Dateien
- Modaler Bestätigungsdialog mit Bootstrap

# Webadressen

- [Blazor WebAssembly](https://dotnet-lexikon.de/Blazor_WebAssembly/lex/9768.aspx) im .NET-Lexikon 
- [Fertige Lösung "MiracleList"](http://miraclelist-bw.azurewebsites.net/) auf Basis Blazor WebAssembly in der Aure-Cloud
- [Cloudbasiertes MiracleList-Backend](http://miraclelistbackend.azurewebsites.net/clientid) basiert auf ASP.NET Core  --> [Quellcode auf GitHub](https://github.com/HSchwichtenberg/MiracleListBackend)
