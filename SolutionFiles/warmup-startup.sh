./curl-amd64 --retry-all-errors --connect-timeout 5 --max-time 5 --retry 10 --retry-delay 2 --retry-max-time 60 -s -I -X GET http://localhost/swagger/v1/swagger.json
./curl-amd64 --retry-all-errors --connect-timeout 5 --max-time 5 --retry 10 --retry-delay 2 --retry-max-time 60 -s -I -X GET http://localhost/api/roles
./curl-amd64 --retry-all-errors --connect-timeout 5 --max-time 5 --retry 10 --retry-delay 2 --retry-max-time 60 -s -I -X GET http://localhost/api/users