# docker build --no-cache -t dotnetcore .
#
FROM microsoft/dotnet:latest
WORKDIR /app
COPY . .

RUN ls -lR

# build
RUN dotnet build

ENTRYPOINT ["dotnet", "run"]