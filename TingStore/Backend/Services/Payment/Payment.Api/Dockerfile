# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app



# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Backend/Services/Payment/Payment.Api/Payment.Api.csproj", "Backend/Services/Payment/Payment.Api/"]
COPY ["Backend/Infrastructure/EventBus.Messages/EventBus.Messages.csproj", "Backend/Infrastructure/EventBus.Messages/"]
COPY ["Backend/Services/Payment/Payment.Application/Payment.Application.csproj", "Backend/Services/Payment/Payment.Application/"]
COPY ["Backend/Services/Payment/Payment.Core/Payment.Core.csproj", "Backend/Services/Payment/Payment.Core/"]
COPY ["Backend/Services/Payment/Payment.Infrastructure/Payment.Infrastructure.csproj", "Backend/Services/Payment/Payment.Infrastructure/"]
RUN dotnet restore "./Backend/Services/Payment/Payment.Api/Payment.Api.csproj"
COPY . .
WORKDIR "/src/Backend/Services/Payment/Payment.Api"
RUN dotnet build "./Payment.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Payment.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Api.dll"]
