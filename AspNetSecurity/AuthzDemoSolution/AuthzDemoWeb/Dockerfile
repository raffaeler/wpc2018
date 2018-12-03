FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["AuthzDemoWeb/AuthzDemoWeb.csproj", "AuthzDemoWeb/"]
RUN dotnet restore "AuthzDemoWeb/AuthzDemoWeb.csproj"
COPY . .
WORKDIR "/src/AuthzDemoWeb"
RUN dotnet build "AuthzDemoWeb.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AuthzDemoWeb.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AuthzDemoWeb.dll"]