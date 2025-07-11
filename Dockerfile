FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY *.sln ./
COPY src/ ./src/

WORKDIR /app/src/AzAutoParking.Api
RUN dotnet restore

RUN dotnet publish -c Realease -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

COPY --from=build /app/publish .

COPY src/AzAutoParking.Api/.env .env
COPY azautoparking.db .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "AzAutoParking.Api.dll"]
