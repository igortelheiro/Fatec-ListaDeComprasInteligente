FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ListaDeComprasInteligente.API/ListaDeComprasInteligente.API.csproj", "ListaDeComprasInteligente.API/"]
COPY ["ListaDeComprasInteligente.Scraper/ListaDeComprasInteligente.Scraper.csproj", "ListaDeComprasInteligente.Scraper/"]
COPY ["ListaDeComprasInteligente.Domain/ListaDeComprasInteligente.Domain.csproj", "ListaDeComprasInteligente.Domain/"]
COPY ["ListaDeComprasInteligente.Service/ListaDeComprasInteligente.Service.csproj", "ListaDeComprasInteligente.Service/"]
RUN dotnet restore "ListaDeComprasInteligente.API/ListaDeComprasInteligente.API.csproj"
COPY . .
WORKDIR "/src/ListaDeComprasInteligente.API"
RUN dotnet build "ListaDeComprasInteligente.API.csproj" -c Release -o /app/build

# RUN chmod +x ./chromium-config.sh && ./chromium-config.sh
# ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome"

FROM build AS publish
RUN dotnet publish "ListaDeComprasInteligente.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ListaDeComprasInteligente.API.dll"]
