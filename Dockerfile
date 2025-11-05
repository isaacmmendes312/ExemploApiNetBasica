# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish "ExemploApiNetBasica.csproj" -c Release -o /app/publish

# Estágio 2: Final
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Comando de entrada
ENTRYPOINT ["dotnet", "ExemploApiNetBasica.dll"]