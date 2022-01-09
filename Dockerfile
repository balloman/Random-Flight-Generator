FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Random Realistic Flight.csproj", "./"]
RUN dotnet restore "Random Realistic Flight.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Random Realistic Flight.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Random Realistic Flight.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Random Realistic Flight.dll"]
