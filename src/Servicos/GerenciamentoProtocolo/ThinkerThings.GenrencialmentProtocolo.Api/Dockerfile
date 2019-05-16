FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/ThinkerThings.GenrencialmentProtocolo.Api/ThinkerThings.GenrencialmentProtocolo.Api.csproj", "src/ThinkerThings.GenrencialmentProtocolo.Api/"]
RUN dotnet restore "src/ThinkerThings.GenrencialmentProtocolo.Api/ThinkerThings.GenrencialmentProtocolo.Api.csproj"
COPY . .
WORKDIR "/src/src/ThinkerThings.GenrencialmentProtocolo.Api"
RUN dotnet build "ThinkerThings.GenrencialmentProtocolo.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ThinkerThings.GenrencialmentProtocolo.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ThinkerThings.GenrencialmentProtocolo.Api.dll"]