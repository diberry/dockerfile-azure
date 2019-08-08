# $ docker build --no-cache -t my-py3-image .

FROM python:3

WORKDIR /usr/src/app

RUN pip install azure-cognitiveservices-language-luis
COPY . .

CMD [ "python", "create-app.py" ]