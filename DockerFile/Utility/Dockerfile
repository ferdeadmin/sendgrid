FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
RUN mkdir -p /data/config/ 
COPY ["Utility.API/Utility.API.csproj", "Utility.API/"]
COPY ["DataAccessLayer/DataAccessLayer.csproj", "DataAccessLayer/"]
COPY ["DomainModel/DomainModel.csproj", "DomainModel/"]
COPY ["BusinessAccessLayer/BusinessAccessLayer.csproj", "BusinessAccessLayer/"]
RUN dotnet restore "Utility.API/Utility.API.csproj"
COPY . .
WORKDIR "/src/Utility.API"
RUN dotnet build "Utility.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Utility.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Utility.API.dll"]
