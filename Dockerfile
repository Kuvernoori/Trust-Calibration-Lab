FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["backend/TrustCalibrationLab.Api/TrustCalibrationLab.Api.csproj", "backend/TrustCalibrationLab.Api/"]
RUN dotnet restore "backend/TrustCalibrationLab.Api/TrustCalibrationLab.Api.csproj"
COPY backend/ backend/
WORKDIR /src/backend/TrustCalibrationLab.Api
RUN dotnet publish "TrustCalibrationLab.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrustCalibrationLab.Api.dll"]
