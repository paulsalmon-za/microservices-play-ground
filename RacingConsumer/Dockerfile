FROM microsoft/dotnet:2.1-sdk-alpine3.7 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine3.7
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "RacingConsumer.dll"]