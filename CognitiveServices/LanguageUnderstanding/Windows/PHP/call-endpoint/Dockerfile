# BUILD IMAGE
# $ docker build --no-cache -t luis-endpoint-php .
#

FROM php:7.2-apache
WORKDIR /go/src/LUIS
COPY . .

ENTRYPOINT ["php","prediction-endpoint-call.php"]