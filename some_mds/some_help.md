Связь между докер-контейнерами https://habr.com/ru/post/554190/
Админка для тарантула https://github.com/basis-company/tarantool-admin

docker network create test_network

docker network connect test_network tarantool_ski_resort

docker network connect test_network dazzling_kepler

docker network inspect test_network

берем ip4 tarantool_ski_resort (172.18.0.2) и используем его в качестве hostname

"ski_admin:Tty454r293300@localhost:3301" -- вместо localhost

вот тут админка http://172.17.0.1:8000/

docker run -e TARANTOOL_CONNECTIONS=ski_admin:Tty454r293300@localhost:3301 -p 8000:80 quay.io/basis-company/tarantool-admin

172.17.0.1 - - [29/Sep/2022:19:35:20 +0000] "POST /admin/api HTTP/1.1" 200 10176 "http://localhost:8000/" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.114 YaBrowser/22.9.1.1095 Yowser/2.5 Safari/537.36"

