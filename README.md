# SI-MiniProject-3

_NOT FINISHED YET: Lists below show which features have been implemented._

- [x] Bank can return confirmation letter and contract to customer
- [x] Banks can respond to request with loan quotes
- [x] Customer can select one loan quote from bank
- [x] Customer can apply for loan based on selected quote
- [ ] Bank can return confirmation letter and contract to customer

# Overview

The system consists of two services, a website, a kafka broker, and a RabbitMq broker.

1. A loan request can be made from the frontend website where its pushed to a Kafka topic. 
2. The bank services can then consume the request from the topic and send a list of loan quotes onto a rabbit mq queue. 
3. The offer service then consumes the loan quotes and creates an offer which it pushes up on a new Kafka topic. 
4. The ui sees this message and shows the active offer to the user. 
5. The user can then select a quote offer and approve it. 
6. ... 

# Build & Run

Run the following command from the root folder to start the application:

```
$ docker-compose up -d
```

_NOTE: The bank services and offer service sometimes crash the first time they start. If this is the case just run the docker compose command again and it should work._

## Access

- Frontend: http://localhost:8080/ - A basic website for making loan requests and accepting offers.
- Kafdrop: http://localhost:9000/ - This can be access to see which topics exists on the kafka broker.

- RabbitMq: http://localhost:15672/ - Default admin panel for RabbitMq (password is the default one)