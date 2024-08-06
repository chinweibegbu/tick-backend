#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TemplateTest/TemplateTest.csproj", "TemplateTest/"]
COPY ["TemplateTest.Infrastructure/TemplateTest.Infrastructure.csproj", "TemplateTest.Infrastructure/"]
COPY ["TemplateTest.Core/TemplateTest.Core.csproj", "TemplateTest.Core/"]
COPY ["TemplateTest.Persistence/TemplateTest.Persistence.csproj", "TemplateTest.Persistence/"]
COPY ["TemplateTest.Domain/TemplateTest.Domain.csproj", "TemplateTest.Domain/"]
RUN dotnet restore "TemplateTest/TemplateTest.csproj"
COPY . .
WORKDIR "/src/TemplateTest"
RUN dotnet build "TemplateTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TemplateTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TemplateTest.dll"]