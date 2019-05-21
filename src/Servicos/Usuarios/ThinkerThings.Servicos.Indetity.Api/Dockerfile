FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Servicos/Usuarios/ThinkerThings.Servicos.Indetity.Api/ThinkerThings.Servicos.Indetity.Api.csproj", "src/Servicos/Usuarios/ThinkerThings.Servicos.Indetity.Api/"]
RUN dotnet restore "src/Servicos/Usuarios/ThinkerThings.Servicos.Indetity.Api/ThinkerThings.Servicos.Indetity.Api.csproj"
COPY . .
WORKDIR "/src/src/Servicos/Usuarios/ThinkerThings.Servicos.Indetity.Api"
RUN dotnet build "ThinkerThings.Servicos.Indetity.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ThinkerThings.Servicos.Indetity.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ThinkerThings.Servicos.Indetity.Api.dll"]