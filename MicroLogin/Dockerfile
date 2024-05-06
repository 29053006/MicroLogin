#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroLogin/MicroLogin.csproj", "MicroLogin/"]
RUN dotnet restore "./MicroLogin/MicroLogin.csproj"
COPY . .
WORKDIR "/src/MicroLogin"
RUN dotnet build "./MicroLogin.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroLogin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroLogin.dll"]
# construir la imagen
# docker build -t micrologin -f MicroLogin/Dockerfile .
# crear y desplegar el contenedor
#                   nombre del contenedor                                                      nombre de la imagen creada previamente
# docker run --name micrologin -e ASPNETCORE_ENVIRONMENT=Development -p 8080:8080 -p 8081-8081 micrologin
# url para visualizar swagger
# http://localhost:8080/swagger/index.html