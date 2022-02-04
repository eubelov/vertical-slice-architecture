FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ProductsApi/ProductsApi.csproj", "ProductsApi/"]
RUN dotnet restore "src/ProductsApi/ProductsApi.csproj"
COPY . .
WORKDIR "/src/ProductsApi"
RUN dotnet build "ProductsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductsApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductsApi.dll"]
