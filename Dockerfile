FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/Pokemon.Api/Pokemon.Api.csproj", "Pokemon.Api/"]
RUN dotnet restore "Pokemon.Api/Pokemon.Api.csproj"
COPY . .
WORKDIR "/src/Pokemon.Api"
RUN dotnet build "../Pokemon.Api/Pokemon.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "../Pokemon.Api/Pokemon.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokemon.Api.dll"]
