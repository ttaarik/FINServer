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

# .csproj-Dateien kopieren und Abhängigkeiten wiederherstellen
COPY *.csproj ./
RUN dotnet restore

# Restliche Dateien kopieren und Projekt erstellen
COPY . ./
RUN dotnet publish -c Release -o out

# Das Runtime-Image erstellen
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Definiere den Einstiegspunkt für das Docker-Image
ENTRYPOINT ["dotnet", "run", "FINServer.dll"]
