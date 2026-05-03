# Docs

- docker-files: store all the docker file of all of the services
- development/docker-compose.yml: one docker conpose to start all the development service

# How to run the docker file to docker image

- cd to root
  docker build -f infrastructure\docker\docker-files\Fakebook.Auth.Dockerfile -t fakebook-auth:v4 .
  docker build -f infrastructure\docker\docker-files\Fakebook.Bff.Dockerfile -t fakebook-bff:v4 .

# Run the docker compose file

- cd to root
  docker compose -f infrastructure\docker\development\docker-compose.yml up -d

# Generate the certifate for service for development only [not use https]

- cd to root (the password should be filename + 123)
  dotnet dev-certs https -ep infrastructure\docker\certs\fakebook-bff.pfx -p fakebook-bff123
  dotnet dev-certs https --trust
