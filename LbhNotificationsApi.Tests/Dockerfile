FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./LbhNotificationsApi.sln ./
COPY ./LbhNotificationsApi/LbhNotificationsApi.csproj ./LbhNotificationsApi/
COPY ./LbhNotificationsApi.Tests/LbhNotificationsApi.Tests.csproj ./LbhNotificationsApi.Tests/

RUN dotnet restore ./LbhNotificationsApi/LbhNotificationsApi.csproj
RUN dotnet restore ./LbhNotificationsApi.Tests/LbhNotificationsApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out LbhNotificationsApi.Tests/LbhNotificationsApi.Tests.csproj

CMD dotnet test
