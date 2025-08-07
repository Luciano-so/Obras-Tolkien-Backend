FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore TerraMedia.Api.sln

WORKDIR /app/TerraMedia.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/TerraMedia.Api/out ./

EXPOSE 7077
ENV ASPNETCORE_URLS=http://+:7077

ENTRYPOINT ["dotnet", "TerraMedia.Api.dll"]
