FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore TerraMedia.Api.sln

WORKDIR /app/TerraMedia.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Atualiza os certificados raiz para HTTPS funcionar corretamente
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates

COPY --from=build /app/TerraMedia.Api/out ./

EXPOSE 7077
ENV ASPNETCORE_URLS=http://+:7077

ENTRYPOINT ["dotnet", "TerraMedia.Api.dll"]
