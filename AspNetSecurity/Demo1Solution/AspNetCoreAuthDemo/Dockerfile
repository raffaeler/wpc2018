FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["AspNetCoreAuthDemo/AspNetCoreAuthDemo.csproj", "AspNetCoreAuthDemo/"]
RUN dotnet restore "AspNetCoreAuthDemo/AspNetCoreAuthDemo.csproj"
COPY . .
WORKDIR "/src/AspNetCoreAuthDemo"
RUN dotnet build "AspNetCoreAuthDemo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AspNetCoreAuthDemo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AspNetCoreAuthDemo.dll"]