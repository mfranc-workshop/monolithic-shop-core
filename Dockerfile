FROM microsoft/dotnet:latest

RUN  mkdir -p /usr/src/monolithic-shop
WORKDIR /usr/src/monolithic-shop

COPY . /usr/src/monolithic-shop
RUN dotnet restore

ENTRYPOINT [ "dotnet", "run" ]
