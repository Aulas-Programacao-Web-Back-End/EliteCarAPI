# ==========================================
# Build
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["EliteCarAPI.csproj", "./"]

RUN dotnet restore "EliteCarAPI.csproj"

COPY . .

RUN dotnet publish "EliteCarAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# ==========================================
# Runtime
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Render utiliza a porta 10000 por padrão
ENV ASPNETCORE_HTTP_PORTS=10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "EliteCarAPI.dll"]