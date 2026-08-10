# ==========================================
# Etapa 1 - Build
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copia o arquivo do projeto
COPY ["EliteCarAPI.csproj", "./"]

# Restaura as dependências
RUN dotnet restore "EliteCarAPI.csproj"

# Copia o restante dos arquivos
COPY . .

# Compila e publica a aplicação
RUN dotnet publish "EliteCarAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# ==========================================
# Etapa 2 - Runtime
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# Copia a aplicação publicada
COPY --from=build /app/publish .

# Faz o ASP.NET Core escutar na porta
# fornecida pelo Render
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# Porta utilizada pelo container
EXPOSE 10000

# Inicia a aplicação
ENTRYPOINT ["dotnet", "EliteCarAPI.dll"]