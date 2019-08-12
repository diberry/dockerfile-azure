# docker build --no-cache -t dotnetcore .
#
FROM microsoft/dotnet:latest
WORKDIR /usr/src/app
COPY . /usr/src/app

RUN ls

# build
RUN dotnet build

ENTRYPOINT ["dotnet", "run"]