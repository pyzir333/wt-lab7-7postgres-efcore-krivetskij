
# Stage 1: build and publish the application.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY wt-lab7-7postgres-efcore-krivetskij.csproj ./
RUN dotnet restore wt-lab7-7postgres-efcore-krivetskij.csproj

COPY . .
RUN dotnet publish wt-lab7-7postgres-efcore-krivetskij.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Stage 2: run the published application in the smaller ASP.NET runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000
ENTRYPOINT ["dotnet", "wt-lab7-7postgres-efcore-krivetskij.dll"]
