#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app

# dependencies needed to run headless browser in container
RUN apt-get update
RUN apt-get install -y wget unzip fontconfig locales gconf-service libasound2 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 libexpat1 libfontconfig1 libgcc1 libgconf-2-4 libgdk-pixbuf2.0-0 libglib2.0-0 libgtk-3-0 libnspr4 libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 libxcomposite1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 ca-certificates fonts-liberation libappindicator1 libnss3 lsb-release xdg-utils wget

# copy solution files
COPY PokemonTranslator.sln .

# copy projects files
COPY ./src/PokemonTranslator.Abstractions/PokemonTranslator.Abstractions.csproj ./src/PokemonTranslator.Abstractions/PokemonTranslator.Abstractions.csproj
COPY ./src/PokemonTranslator.API/PokemonTranslator.API.csproj ./src/PokemonTranslator.API/PokemonTranslator.API.csproj
COPY ./src/PokemonTranslator.DTO/PokemonTranslator.DTO.csproj ./src/PokemonTranslator.DTO/PokemonTranslator.DTO.csproj
COPY ./src/PokemonTranslator.Services/PokemonTranslator.Services.csproj ./src/PokemonTranslator.Services/PokemonTranslator.Services.csproj

# copy test projects files
COPY ./tests/PokemonTranslator.API.Tests/PokemonTranslator.API.Tests.csproj ./tests/PokemonTranslator.API.Tests/PokemonTranslator.API.Tests.csproj
COPY ./tests/PokemonTranslator.Services.Tests/PokemonTranslator.Services.Tests.csproj ./tests/PokemonTranslator.Services.Tests/PokemonTranslator.Services.Tests.csproj
COPY ./tests/PokemonTranslator.Services.IntegrationTests/PokemonTranslator.Services.IntegrationTests.csproj ./tests/PokemonTranslator.Services.IntegrationTests/PokemonTranslator.Services.IntegrationTests.csproj

# nuget restore
RUN dotnet restore

# copy everything else (excluding content specified in .dockerignore)
COPY . .

# read configuration and store into env variables
ARG buildConfiguration=Release
ENV BUILD_CONFIGURATION=${buildConfiguration}

# build solution
RUN dotnet build "PokemonTranslator.sln" -c ${buildConfiguration} -o /app/build

# run unit tests
RUN dotnet test "./tests/PokemonTranslator.API.Tests/PokemonTranslator.API.Tests.csproj" --no-restore -c ${buildConfiguration}
RUN dotnet test "./tests/PokemonTranslator.Services.Tests/PokemonTranslator.Services.Tests.csproj" --no-restore -c ${buildConfiguration}

# run integration tests
RUN dotnet test "./tests/PokemonTranslator.Services.IntegrationTests/PokemonTranslator.Services.IntegrationTests.csproj" --no-restore -c ${buildConfiguration}

FROM build AS publish
RUN dotnet publish "./src/PokemonTranslator.API/PokemonTranslator.API.csproj" -c ${buildConfiguration} -o /app/publish

FROM base AS final
WORKDIR /app

# dependencies needed to run headless browser in container
RUN apt-get update
RUN apt-get install -y wget unzip fontconfig locales gconf-service libasound2 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 libexpat1 libfontconfig1 libgcc1 libgconf-2-4 libgdk-pixbuf2.0-0 libglib2.0-0 libgtk-3-0 libnspr4 libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 libxcomposite1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 ca-certificates fonts-liberation libappindicator1 libnss3 lsb-release xdg-utils wget

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokemonTranslator.API.dll"]