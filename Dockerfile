#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app

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
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokemonTranslator.API.dll"]