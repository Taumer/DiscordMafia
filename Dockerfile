FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine-$TARGETARCH AS build-env
RUN apk --update add nodejs npm git
RUN npm install -g bower
RUN echo '{ "allow_root": true }' > /root/.bowerrc
COPY . /opt/sources
WORKDIR /opt/sources
RUN dotnet restore
RUN dotnet build
RUN dotnet publish -c Release -o out

ARG IMAGE_amd64="3.1-alpine"
ARG IMAGE_arm64="3.1-alpine-arm64"

FROM mcr.microsoft.com/dotnet/runtime:$IMAGE_$TARGETARCH as bot
COPY --from=build-env /opt/sources/out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "DiscordMafia.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:$IMAGE_$TARGETARCH as web
COPY --from=build-env /opt/sources/out /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MafiaWeb.dll"]