FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Progress.Api/Progress.Api.fsproj", "Progress.Api/"]
COPY ["Progress.Repository/Progress.Repository.fsproj", "Progress.Repository/"]
COPY ["Progress.Context/Progress.Context.fsproj", "Progress.Context/"]
COPY ["Progress.Domain/Progress.Domain.fsproj", "Progress.Domain/"]
COPY ["Progress.Business/Progress.Business.fsproj", "Progress.Business/"]
RUN dotnet restore "Progress.Api/Progress.Api.fsproj"
COPY . .
WORKDIR "/src/Progress.Api"
RUN dotnet build "Progress.Api.fsproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Progress.Api.fsproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Progress.Api.dll"]