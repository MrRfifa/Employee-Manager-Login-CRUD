FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore --disable-parallel

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app

COPY --from=build /app/out ./

EXPOSE 80

# Optional: Set environment variables for the API
# ENV ASPNETCORE_ENVIRONMENT=Production
# ENV ASPNETCORE_URLS=http://*:80
ENTRYPOINT [ "dotnet","Backend.dll" ]

# CMD ["./Backend"]
