cls
cd C:\Projects\githubRepos\DockerComposeDemo
docker build -t nishantnepal/k8s-svc-a -f ./WepAppA/Dockerfile .
docker build -t nishantnepal/k8s-svc-b -f ./WebAppB/Dockerfile .
docker build -t nishantnepal/k8s-svc-c -f ./WebAppC/Dockerfile .

docker push nishantnepal/k8s-svc-a
docker push nishantnepal/k8s-svc-b
docker push nishantnepal/k8s-svc-c