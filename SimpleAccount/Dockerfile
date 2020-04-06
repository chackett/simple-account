FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 3000
EXPOSE 3001
ENV ASPNETCORE_URLS=http://*:3000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["SimpleAccount.csproj", ""]
RUN dotnet restore "SimpleAccount.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SimpleAccount.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleAccount.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleAccount.dll"]
