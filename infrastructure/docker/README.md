# Docs

- docker-files: store all the docker file of all of the services
- development/docker-compose.yml: one docker conpose to start all the development service

# How to run the docker file to docker image

- cd to root
  docker build -f infrastructure\docker\docker-files\Fakebook.Auth.Dockerfile -t fakebook-auth:v1 .

# Run the docker compose file

- cd to root
  docker compose -f infrastructure\docker\development\docker-compose.yml up -d

# Generate the certifate for service for development only

- cd to root (the password should be filename + 123)
  dotnet dev-certs https -ep infrastructure\docker\certs\fakebook-auth.pfx -p fakebook-auth123
  dotnet dev-certs https --trust
