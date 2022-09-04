FROM mcr.microsoft.com/dotnet/runtime:3.1
COPY ./publish /app
WORKDIR /app
ENTRYPOINT ["dotnet", "DiscordMafia.dll"]