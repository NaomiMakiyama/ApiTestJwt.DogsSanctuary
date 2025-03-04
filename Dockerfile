﻿FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /runtime-app
COPY --from=build-env /App/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DogsSanctuary.dll"]