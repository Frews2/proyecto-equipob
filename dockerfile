FROM node:14
WORKDIR /node
COPY package.json package-lock.json ./

RUN npm install
WORKDIR /node/app
COPY . .
CMD [ "npm", "start" ]