# Introduction 
Sample Asp.Net.Core web API application to excercise TDD, Dependency Injection & Docker principles.

# Getting Started
1.	Run
	- Execute docker-build.sh
	- Execute docker-run.sh
	- Navigate to http://localhost:3000
	
2.	Software dependencies
	- .NET Core 3.1
	
3.	API references
	- Swagger endpoint available at http://localhost:3000/index.html

# Description
The GET Api pokemon/<pokemon-name> search a pokemon in the configured repository, and returns its translated description.
The translation is done using the configured provider.

# Available repository service types
	- PokeAPI repository
		Use PokeApi as source repository https://pokeapi.co/
	- Fake repository
		Return a fake description containing the given pokemon name

	PokeAPI is the default configuration.
	It is possible to override the configuration directly in the appsettings.json
	(key RepositoryServiceType)

# Available translation service types
	- Fake translation
		Simply converts the description to upper case text
	- Shakespeare translation - NOT IMPLEMENTED
		Return shakespeare description using api from https://funtranslations.com/shakespeare

	Fake is the default configuration.
	It is possible to override the configuration directly in the appsettings.json (key TranslationServiceType)
	Configuring Shakespeare will lead to a not implemented excpetion
	
# Overriding configurations with docker-build
It is possible to override any configuration using env variables.

For example to run the docker image configuring a Fake repository service you can do the following:

	docker run -e "RepositoryServiceType=Fake"
