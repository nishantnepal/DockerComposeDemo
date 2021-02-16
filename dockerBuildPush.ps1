cls
cd C:\Projects\githubRepos\DockerComposeDemo
docker build -t nishantnepal/k8s-svc-a -f ./WepAppA/DockerFile .
docker build -t nishantnepal/k8s-svc-b -f ./WebAppB/DockerFile .
docker build -t nishantnepal/k8s-svc-c -f ./WebAppC/DockerFile .

docker push nishantnepal/k8s-svc-a
docker push nishantnepal/k8s-svc-b
docker push nishantnepal/k8s-svc-c