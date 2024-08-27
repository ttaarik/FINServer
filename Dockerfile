# Verwende das offizielle .NET Core SDK Image als Build-Umgebung
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Kopiere alle C#-Dateien
COPY *.cs ./

# Führe das C#-Skript direkt aus
ENTRYPOINT ["dotnet", "run", "FINServer.sln"]
