FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./HackneyVaccinationsApi.sln ./
COPY ./HackneyVaccinationsApi/HackneyVaccinationsApi.csproj ./HackneyVaccinationsApi/
COPY ./HackneyVaccinationsApi.Tests/HackneyVaccinationsApi.Tests.csproj ./HackneyVaccinationsApi.Tests/

RUN dotnet restore ./HackneyVaccinationsApi/HackneyVaccinationsApi.csproj
RUN dotnet restore ./HackneyVaccinationsApi.Tests/HackneyVaccinationsApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out HackneyVaccinationsApi.Tests/HackneyVaccinationsApi.Tests.csproj

CMD dotnet test
