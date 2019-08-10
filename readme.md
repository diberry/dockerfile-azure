# Azure Development Environments

This is a collection of development environments I use. Please feel free to contribute, or suggest corrections.

## Use docker and bash shell

1. For each environment, use a docker file, `Dockerfile` that spins up the environment and installs dependencies. Docker container should go away when file finishes. 
1. Create a bash shell script to run the docker command that pulls in any Azure resources such as keys and regions. 

## Docker commands

### List images

docker images

### Remove image

docker rmi image-name-or-id -f

### List containers

docker ps
docker ps -a
