FROM node:14-alpine as build-stage

ARG APP_ENV

ENV DISABLE_OPENCOLLECTIVE=1

WORKDIR /app/

COPY package.json .
COPY package-lock.json .

RUN npm ci

COPY src src
COPY public public
COPY .env.$APP_ENV .env
COPY tsconfig.json .

RUN npm run build

FROM nginx:1.21.5-alpine

COPY --from=build-stage /app/build/ /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
