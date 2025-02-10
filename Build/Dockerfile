FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ./../shoe-grab-product/*.csproj ./shoe-grab-product/
COPY ./../shoe-grab-common/ ./shoe-grab-common/
COPY ./../shoe-grab-product/ ./shoe-grab-product/

RUN dotnet restore ./shoe-grab-product/*.csproj
RUN dotnet publish ./shoe-grab-product/*.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /out .
EXPOSE 10021
EXPOSE 10022
ENV DB_CONNECTION_STRING="Host=192.168.0.121;Port=30002;Database=db2;Username=admin;Password=admin"
ENTRYPOINT ["dotnet", "ShoeGrabProductManagement.dll"]
