FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Servicos/Usuarios/ThinkerThings.Servicos.Usuarios.Identity.Api/ThinkerThings.Servicos.Usuarios.Identity.Api.csproj", "src/Servicos/Usuarios/ThinkerThings.Servicos.Usuarios.Identity.Api/"]
RUN dotnet restore "src/Servicos/Usuarios/ThinkerThings.Servicos.Usuarios.Identity.Api/ThinkerThings.Servicos.Usuarios.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/Servicos/Usuarios/ThinkerThings.Servicos.Usuarios.Identity.Api"
RUN dotnet build "ThinkerThings.Servicos.Usuarios.Identity.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ThinkerThings.Servicos.Usuarios.Identity.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ThinkerThings.Servicos.Usuarios.Identity.Api.dll"]