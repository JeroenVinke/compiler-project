FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs

WORKDIR /src
COPY . .

WORKDIR "./src/JeroenCompilerFrontend"
RUN dotnet restore "./JeroenCompilerFrontend.csproj"

WORKDIR "./src/JeroenCompilerFrontend"
RUN dotnet publish "JeroenCompilerFrontend.csproj" -c Release -o /app/publish

WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "JeroenCompilerFrontend.dll"]