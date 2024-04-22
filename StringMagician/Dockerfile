FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StringMagician/StringMagician.csproj", "StringMagician/"]
RUN dotnet restore "StringMagician/StringMagician.csproj"
COPY . .
WORKDIR "/src/StringMagician"
RUN dotnet build "StringMagician.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StringMagician.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StringMagician.dll"]
