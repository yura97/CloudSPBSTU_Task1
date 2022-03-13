curl -X GET http://192.168.5.10:5000/api/lector
curl -X POST "http://192.168.5.10:5000/api/lector" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"id\":0,\"name\":\"New One\",\"email\":\"no@ya.ru\"}"
curl -X GET http://192.168.5.10:5000/api/lector
curl -X PUT "http://192.168.5.10:5000/api/lector/1" -H  "accept: text/plain" -H  "Content-Type: application/json" -d "{\"id\":0,\"name\":\"Change\",\"email\":\"asdaf@mail.ru\"}"
curl -X GET http://192.168.5.10:5000/api/lector
curl -X DELETE "http://192.168.5.10:5000/api/lector/4" -H  "accept: */*"
curl -X GET http://192.168.5.10:5000/api/lector
