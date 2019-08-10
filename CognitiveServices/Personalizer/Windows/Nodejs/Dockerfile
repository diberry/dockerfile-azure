# BUILD IMAGE
# $ docker build --no-cache -t my-nodejs-image .

FROM node:10

RUN node --version
RUN npm --version

# example of debug

WORKDIR /app
RUN pwd
RUN ls ..
RUN ls
COPY package*.json ./
RUN ls
RUN pwd
RUN npm install
RUN pwd
RUN ls -al node_modules
COPY rank-and-reward.js .
RUN ls


CMD [ "node", "rank-and-reward.js" ]