FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Servicos/Gerenciador.Usuarios/ThinkerThings.Servicos.Gerenciador.Usuarios/ThinkerThings.Servicos.Gerenciador.Usuarios.csproj", "src/Servicos/Gerenciador.Usuarios/ThinkerThings.Servicos.Gerenciador.Usuarios/"]
RUN dotnet restore "src/Servicos/Gerenciador.Usuarios/ThinkerThings.Servicos.Gerenciador.Usuarios/ThinkerThings.Servicos.Gerenciador.Usuarios.csproj"
COPY . .
WORKDIR "/src/src/Servicos/Gerenciador.Usuarios/ThinkerThings.Servicos.Gerenciador.Usuarios"
RUN dotnet build "ThinkerThings.Servicos.Gerenciador.Usuarios.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ThinkerThings.Servicos.Gerenciador.Usuarios.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ThinkerThings.Servicos.Gerenciador.Usuarios.dll"]