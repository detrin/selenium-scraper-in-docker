# selenium-scraper-in-docker (python version)
Examples of a selenium scraper service in python with Redis, Flask API and Ngrok.

## How does it work?
Chromedriver with selenium run in chrome docker image. Scraper docker image calls selenium running on the port 4444 on backend network and uploads scraped content to redis running on the port 6379. Docker image api is used for serving scraped content. In the case of the `docker-compose-ngrok.yml` ngrok running in a separate docker image is used to tunnel the requests for running API. Check out [ngrok](https://ngrok.com/).
![topology](https://i.imgur.com/S63z08w.png)
User agent can be changed in `appsettings.json` and NGROK_AUTH enviroment has to be added in `docker-compose-ngrok.yml` in order to use ngrok.

## Instalation
Make sure you have installed `docker` and `docker-compose`.

## Usage
To run the `docker-compose` without ngrok hosting on the localhost run 
```shell
docker-compose -f docker-compose.yml up --build
```
To run the `docker-compose` with ngrok run
```shell
docker-compose -f docker-compose-ngrok.yml up --build
```
Visit your ngrok account to see the new http endpoint.