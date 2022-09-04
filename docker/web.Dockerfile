FROM mcr.microsoft.com/dotnet/aspnet:3.1
COPY ./publish /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MafiaWeb.dll"]