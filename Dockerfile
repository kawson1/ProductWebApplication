FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /productApp
COPY ./ProductWebAPI ./API/
COPY ./ProductWebApplication ./Web/

RUN dotnet restore ./API
RUN dotnet publish -o /API/published-files

RUN dotnet restore ./ProductWebApplication
RUN dotnet publish -o /Web/published-files

ENTRYPOINT [ "dotnet", "/app/API/published-files/ProductWebAPI.dll" ]
# ENTRYPOINT [ "dotnet", "/app/Web/published-files/ProductWebApplication.dll" ]
