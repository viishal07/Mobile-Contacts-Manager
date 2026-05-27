FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ContactManager.API.csproj", "."]
RUN dotnet restore "./ContactManager.API.csproj"
COPY . .
RUN dotnet build "ContactManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactManager.API.dll"]
