FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["GAB2019.Inception.Web/GAB2019.Inception.Web.csproj", "GAB2019.Inception.Web/"]
RUN dotnet restore "GAB2019.Inception.Web/GAB2019.Inception.Web.csproj"
COPY . .
WORKDIR "/src/GAB2019.Inception.Web"
RUN dotnet build "GAB2019.Inception.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GAB2019.Inception.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GAB2019.Inception.Web.dll"]