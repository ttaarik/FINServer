# Verwende das offizielle .NET Core SDK Image als Build-Umgebung
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Kopiere csproj und restore die Abhängigkeiten
COPY *.csproj ./
RUN dotnet restore

# Kopiere den Rest des Codes und baue das Projekt
COPY . ./
RUN dotnet publish -c Release -o out

# Verwende das offizielle .NET Core Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Starte die Anwendung
ENTRYPOINT ["dotnet", "FINServer.dll"]
