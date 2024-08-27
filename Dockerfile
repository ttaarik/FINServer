# Verwende das offizielle .NET Core SDK Image als Build-Umgebung (working)
# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
#WORKDIR /app

# Kopiere alle C#-Dateien
#COPY *.csproj ./

# Führe das C#-Skript direkt aus
#ENTRYPOINT ["dotnet", "run", "FINServer.sln"]
# Verwenden des .NET SDK als Basis-Image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Arbeitsverzeichnis erstellen
WORKDIR /app

# Kopiere die Projektdateien
COPY *.cs ./


# Definiere den Einstiegspunkt für das Docker-Image
ENTRYPOINT ["dotnet", "run", "FINServer.dll"]
