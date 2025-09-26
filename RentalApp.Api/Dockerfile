# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy
COPY ["RentalApp.Api/RentalApp.Api.csproj", "RentalApp.Api/"]
COPY ["RentalApp.Core/RentalApp.Core.csproj", "RentalApp.Core/"]
COPY ["RentalApp.Infrastructure/RentalApp.Infrastructure.csproj", "RentalApp.Infrastructure/"]
COPY ["RentalApp.Messaging/RentalApp.Messaging.csproj", "RentalApp.Messaging/"]

# Restore dependencies
RUN dotnet restore "RentalApp.Api/RentalApp.Api.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/RentalApp.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose ports
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "RentalApp.Api.dll"]