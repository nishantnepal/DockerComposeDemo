cls
cd C:\Projects\githubRepos\DockerComposeDemo
docker build -t nishantnepal/demosvc-a:v1 -f ./WepAppA/DockerFile .
docker build -t nishantnepal/demosvc-b:v1 -f ./WebAppB/DockerFile .
docker build -t nishantnepal/demosvc-c:v1 -f ./WebAppC/DockerFile .

docker push nishantnepal/demosvc-a:v1
docker push nishantnepal/demosvc-b:v1
docker push nishantnepal/demosvc-c:v1