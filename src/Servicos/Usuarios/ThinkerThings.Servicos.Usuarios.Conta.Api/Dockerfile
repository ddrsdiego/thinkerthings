FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Servicos/Usuarios/ThinkerThings.Servicos.Usuarios.Conta.Api/ThinkerThings.Servicos.Usuarios.Conta.Api.csproj", "src/Servicos/Usuarios/ThinkerThings.Servicos.Usuarios.Conta.Api/"]
RUN dotnet restore "src/Servicos/Usuarios/ThinkerThings.Servicos.Usuarios.Conta.Api/ThinkerThings.Servicos.Usuarios.Conta.Api.csproj"
COPY . .
WORKDIR "/src/src/Servicos/Usuarios/ThinkerThings.Servicos.Usuarios.Conta.Api"
RUN dotnet build "ThinkerThings.Servicos.Usuarios.Conta.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ThinkerThings.Servicos.Usuarios.Conta.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ThinkerThings.Servicos.Usuarios.Conta.Api.dll"]