FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 82
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MSPrivateLibrary.csproj", "./"]
RUN dotnet restore "MSPrivateLibrary.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "MSPrivateLibrary.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSPrivateLibrary.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSPrivateLibrary.dll"]