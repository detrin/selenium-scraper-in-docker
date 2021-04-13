# selenium-scraper-in-docker
Examples of a selenium scraper service in python and C# with Redis, Flask API and Ngrok.

## How does it work?
Chromedriver with selenium run in chrome docker image. Scraper docker image calls selenium running on the port 4444 on backend network and uploads scraped content to redis running on the port 6379. Docker image api is used for serving scraped content. In the case of the `docker-compose-ngrok.yml` ngrok running in a separate docker image is used to tunnel the requests for running API. Check out [ngrok](https://ngrok.com/).
![topology](https://i.imgur.com/S63z08w.png)
User agent can be changed in `appsettings.json` and NGROK_AUTH enviroment has to be added in `docker-compose-ngrok.yml` in order to use ngrok.

## Checkout
[python scraper](https://github.com/detrin/selenium-scraper-in-docker/tree/main/python)