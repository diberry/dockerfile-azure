# docker build --no-cache -t dotnetcore .
#
FROM microsoft/dotnet:latest
WORKDIR /app
COPY . /app

RUN ls

# build
RUN dotnet build

ENTRYPOINT ["dotnet", "run"]