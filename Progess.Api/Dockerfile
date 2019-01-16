FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Progess.Api/Progess.Api.fsproj", "Progess.Api/"]
RUN dotnet restore "Progess.Api/Progess.Api.fsproj"
COPY . .
WORKDIR "/src/Progess.Api"
RUN dotnet build "Progess.Api.fsproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Progess.Api.fsproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Progess.Api.dll"]