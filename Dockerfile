FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
RUN apt install nodejs npm
RUN npm install -g bower
RUN echo '{ "allow_root": true }' > /root/.bowerrc
COPY . /opt/sources
WORKDIR /opt/sources
RUN dotnet restore
RUN dotnet build
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:3.1 as bot
COPY --from=build-env /opt/sources/out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "DiscordMafia.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:3.1 as web
COPY --from=build-env /opt/sources/out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MafiaWeb.dll"]