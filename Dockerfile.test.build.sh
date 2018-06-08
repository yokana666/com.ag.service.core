docker build -f Dockerfile.test.build -t com-danliris-service-core-webapi:test-build .
docker create --name com-danliris-service-core-webapi-test-build com-danliris-service-core-webapi:test-build
mkdir bin
docker cp com-danliris-service-core-webapi-test-build:/out/. ./bin/publish
docker build -f Dockerfile.test -t com-danliris-service-core-webapi:test .