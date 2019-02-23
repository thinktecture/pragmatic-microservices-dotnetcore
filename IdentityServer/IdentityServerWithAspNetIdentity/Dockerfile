FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY IdentityServerWithAspNetIdentity/IdentityServerWithAspNetIdentity.csproj IdentityServerWithAspNetIdentity/
RUN dotnet restore IdentityServerWithAspNetIdentity/IdentityServerWithAspNetIdentity.csproj
COPY . .
WORKDIR /src/IdentityServerWithAspNetIdentity
RUN dotnet build IdentityServerWithAspNetIdentity.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish IdentityServerWithAspNetIdentity.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IdentityServerWithAspNetIdentity.dll"]
