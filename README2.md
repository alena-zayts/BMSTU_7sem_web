# BMSTU_7sem_web
7th sem BMSTU, Web

Пума (шарп, создание игры змейка.), к зачету

https://github.com/Winterpuma/bmstu_web/tree/main  c#

https://github.com/Sunshine-ki/BMSTU7_WEB c#

https://github.com/llviolall/Web-sem-project ruby+java

https://github.com/Justarone/bmstu-web java

https://github.com/fairay/CourseWeb go 

https://github.com/oljakon/web python

https://github.com/ansushina/web_bmstu python

https://github.com/anastasialavrova/bmstu_web python



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

