FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build-env
RUN apk --update add nodejs npm git
RUN npm install -g bower
RUN echo '{ "allow_root": true }' > /root/.bowerrc
COPY . /opt/sources
WORKDIR /opt/sources
RUN dotnet restore
RUN dotnet build
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:3.1-alpine as bot
COPY --from=build-env /opt/sources/DiscordMafia/out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "DiscordMafia.dll"]

FROM mcr.microsoft.com/dotnet/runtime:3.1-alpine as web
COPY --from=build-env /opt/sources/MafiaWeb/out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MafiaWeb.dll"]