#### .Net Simple Auth

--ABOUT--

A .Net Core 3.1 application with a simple JWT Authorization that can upload log files

--INSTRUCTIONS--

To run the project you need a working and running PostgreSQL database. The project will automatically migrate all neccessary tables once it starts. To change connection settings, you need to edit appsettings
in "NetPOC-Backend\NetSimpleAuth.Backend.API\appsettings.json". If you need to change the database settings, you can access it with the following password: "password"

Alternativelly, you can build a Docker image with the docker-compose file that comes with the project.

The default swagger endpoint is "http://localhost:5000/swagger"

--ADDITIONAL INFO--

The project uses Dapper to make the database queries and FluentMigrator to make the migrations. Of course, migrations are not a good idea to be used in a production environment, but for the purpose of this project
it will cause no harm. The authentication uses a sha256 hashed password with random salt, so a security breach will be very unlikely, unless the database gets compromised. Some peculiarities are the generic CRUD repository
that can be fully reutilized by any data entity following DRY principles and the layered Hexagonal Architecture that decouples logic from infrastructure.
    
Everything can be changed without any major refactoring
