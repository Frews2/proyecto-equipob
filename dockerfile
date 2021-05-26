FROM node:14
WORKDIR /node
COPY package.json package-lock.json ./

RUN npm install
RUN npm i express-fileupload
WORKDIR /node/app
COPY . .
CMD [ "npm", "start" ]