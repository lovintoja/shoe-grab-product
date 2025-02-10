FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ./../shoe-grab-product/*.csproj ./shoe-grab-product/
RUN dotnet restore ./shoe-grab-product/*.csproj
COPY ./../shoe-grab-product/ ./shoe-grab-product/
RUN dotnet publish ./shoe-grab-product/*.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /out .
EXPOSE 10021
EXPOSE 10022
ENV DB_CONNECTION_STRING="Host=your-db-vm-ip;Port=5432;Database=your-db-name;Username=your-username;Password=your-password;"
ENTRYPOINT ["dotnet", "shoe-grab-product.dll"]
