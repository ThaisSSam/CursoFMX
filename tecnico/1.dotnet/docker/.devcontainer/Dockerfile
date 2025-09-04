# Usa imagem do SDK .NET 8 como base
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Instala o debugger do VS Code e outras ferramentas (opcional)
RUN apt-get update \
    && apt-get install -y unzip curl git \
    && rm -rf /var/lib/apt/lists/*
 