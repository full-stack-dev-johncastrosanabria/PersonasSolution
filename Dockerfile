# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["Personas.Api/Personas.Api.vbproj", "Personas.Api/"]
COPY ["Personas.Business/Personas.Business.vbproj", "Personas.Business/"]
COPY ["Personas.Repository/Personas.Repository.vbproj", "Personas.Repository/"]
COPY ["Personas.EF/Personas.EF.vbproj", "Personas.EF/"]

# Restore dependencies
RUN dotnet restore "Personas.Api/Personas.Api.vbproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/Personas.Api"
RUN dotnet build "Personas.Api.vbproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Personas.Api.vbproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Personas.Api.dll"]
