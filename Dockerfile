FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ./NLayer.Web/*.csproj ./NLayer.Web/
COPY ./NLayer.Core/*.csproj ./NLayer.Core/
COPY ./NLayer.Data/*.csproj ./NLayer.Data/
COPY ./NLayer.Service/*.csproj ./NLayer.Service/
COPY ./NLayer.Web/*.csproj ./NLayer.Web/
COPY *.sln .
RUN dotnet restore
COPY . .
RUN dotnet publish ./NLayer.Web/*.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .
ENV ASPNETCORE_URLS="http://*:5000"
ENTRYPOINT ["dotnet", "NLayer.Web.dll"]
