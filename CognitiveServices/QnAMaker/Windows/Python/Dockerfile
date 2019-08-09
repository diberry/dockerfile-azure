# $ docker build --no-cache -t my-py3-qnamaker-image .

FROM python:3

WORKDIR /usr/src/app

RUN pip install azure-cognitiveservices-knowledge-qnamaker
COPY . .
RUN ls

CMD [ "python", "knowledgebase_quickstart.py" ]