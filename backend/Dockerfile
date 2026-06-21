FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY WorkPlanner.csproj ./
RUN dotnet restore WorkPlanner.csproj
COPY . .
RUN dotnet publish WorkPlanner.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "WorkPlanner.dll"]
