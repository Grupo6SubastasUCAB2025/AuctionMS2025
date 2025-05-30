FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "MSAuction/MSAuction.csproj"
RUN dotnet build "MSAuction/MSAuction.csproj" -c Release -o /app/build
RUN dotnet publish "MSAuction/MSAuction.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MSAuction.dll"]