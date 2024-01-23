docker network create tulip-net || true
docker build -f Lab.Gym.Web/Dockerfile -t gymweb-img .
docker rm -f web-gym
docker run --restart unless-stopped --name web-gym --net tulip-net -d -p 5002:8080 gymweb-img
