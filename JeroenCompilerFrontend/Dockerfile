FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs

WORKDIR /src
COPY . .

# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-restore?tabs=netcore2x
RUN dotnet restore "./JeroenCompilerFrontend.csproj"

# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore21
RUN dotnet publish "JeroenCompilerFrontend.csproj" -c Release -o /app/publish

# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim

EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "JeroenCompilerFrontend.dll"]