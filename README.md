# pokemon_shakespeare

Author: Suthathip Kaewsamsee
Version: 1.0

This API provides the 'shakespeare' description of a given Pokemon character by its name. The API utilises the following public APIs:

| API                   | Link                                       |
| :---:                 | :-:                                        |
| PokéAPI:              | https://pokeapi.co/                        |
| Shakespeare translator| https://funtranslations.com/api/shakespeare|


The API only supports English. 

## Example

Requests: 

```
http://localhost:3000/pokemon/charizard
```

Responses:

```
{
  "name": "charizard",
  "description": "Spits fire yond is hot enow to melt boulders. Known to cause forest fires unintentionally. At which hour expelling a fie of super hot fire,  the red flame at the tip of its tail burns moo intensely. If 't be true charizard beest­ cometh furious,  the flame at the tip of its tail flares up in a whitish- blue color. Breathing intense,  hot flames,  't can melt almost any­ thing. Its breath inflicts lacking valor teen on enemies.'t uses its wings to fly high. The temperature of its fire increases as 't gains exper­ ience in hurlyburly."
}
```


## Pre-requisites

- Docker

The project requires Docker to be set up locally on the machine to be runned as a Docker container or via Docker Compose. Follow this link https://docs.docker.com/get-docker/ to set up Docker.


- Clone the project

Clone the project to the local machine through preferred method, such as running a git command:

```
git clone https://github.com/ploysuthathip/pokemon_shakespeare.git 
```

## Running the project


### With Docker Compose

If the machine is already set up with Docker, then Docker Compose can be used to easily start up the application. 

1) First navigate to the root directory of the project
2) Enter the following command to start Docker Compose

```
docker compose up
```
This will spins up the Docker container without the need to manually build the image first.

3) Ensure that the application is up and running

- Using the provided healthcheck by making a GET call (can be from the preferred browser)

```
http://localhost:3000/healthcheck
```
- Or navigate to the swagger documentation through the perferred browser

```
http://localhost:3000/swagger/index.html
```

4) Make the API call by passing the name of the Pokemon character, such as via Swagger or browser, or Postman

```
http://localhost:3000/pokemon/charizard
```

## Assumptions

- Level of information

The GET endpoint that returns the shakespear translation of the Pokemon character takes the assumption of only taking the top 5 paragraphs from the PokéAPI. This is to provide just enough information without being overwhelming.


## Improvements

- Logging
The application provides some basic logging through the console with Ilogger. It could benefits from a more sophisicated logging engine such as Azure Appication Insights (if to be hosted in Azure), 

- Validation
Validation was not implemented at this point due to the simplicity of the API. Validations could be implemented to ensure that the correct format of the input is provided, for example, checking that the character 'name' parameter contains only valid letters (no numerical values) etc.

- Tests (more unit tests and integration tests)
Add more 

- Ratelimiting
This could be implemented to prevent availability issues.

