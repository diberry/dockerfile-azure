# $ docker build --no-cache -t my-py3-personalizer-image .

FROM python:3

WORKDIR /usr/src/app

RUN pip install azure-cognitiveservices-personalizer
COPY . .
RUN ls

CMD [ "python", "rank-and-reward.py" ]