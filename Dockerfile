# Stage 1: Build Vue Client
FROM node:20-alpine AS client-builder
WORKDIR /app/keepr.client
COPY keepr.client/package*.json ./
RUN npm install
COPY keepr.client/ ./
RUN npm run build

# Stage 2: Build .NET Web API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dotnet-builder
WORKDIR /app
COPY keepr/keepr.csproj keepr/
RUN dotnet restore keepr/keepr.csproj
COPY keepr/ keepr/
# Copy static built frontend files into wwwroot
COPY --from=client-builder /app/keepr/wwwroot keepr/wwwroot
WORKDIR /app/keepr
RUN dotnet publish -c Release -o /app/publish

# Stage 3: Final Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=dotnet-builder /app/publish .

# Cloud providers dynamically populate the PORT environment variable
ENV PORT=8080
CMD ["sh", "-c", "ASPNETCORE_URLS=http://*:${PORT} dotnet keepr.dll"]
