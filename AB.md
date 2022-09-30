-n – количество запросов, которые нужно выполнить для сеанса бенчмаркинга.

-c – это параллелизм, который обозначает количество одновременных запросов. 

## С балансировкой

### n=100, c=10

```
sudo ab -n 100 -c 10 http://127.0.0.1/api/v1/account/
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking 127.0.0.1 (be patient).....done


Server Software:        Kestrel
Server Hostname:        127.0.0.1
Server Port:            80

Document Path:          /api/v1/account/
Document Length:        323 bytes

Concurrency Level:      10
Time taken for tests:   12.752 seconds
Complete requests:      100
Failed requests:        0
Total transferred:      46200 bytes
HTML transferred:       32300 bytes
Requests per second:    7.84 [#/sec] (mean)
Time per request:       1275.197 [ms] (mean)
Time per request:       127.520 [ms] (mean, across all concurrent requests)
Transfer rate:          3.54 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.2      0       2
Processing:   677 1195 139.2   1156    1629
Waiting:      676 1195 139.2   1156    1629
Total:        677 1195 139.3   1156    1629

Percentage of the requests served within a certain time (ms)
  50%   1156
  66%   1208
  75%   1247
  80%   1272
  90%   1400
  95%   1515
  98%   1603
  99%   1629
 100%   1629 (longest request)
```




%### n=100, c=20
%```
%```

## Без балансировки

### n=1000, c=10

```
sudo ab -n 1000 -c 10 http://127.0.0.1/api/v1/account/
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking 127.0.0.1 (be patient)
Completed 100 requests
Completed 200 requests
Completed 300 requests
Completed 400 requests
Completed 500 requests
Completed 600 requests
Completed 700 requests
Completed 800 requests
Completed 900 requests
Completed 1000 requests
Finished 1000 requests


Server Software:        Kestrel
Server Hostname:        127.0.0.1
Server Port:            80

Document Path:          /api/v1/account/
Document Length:        323 bytes

Concurrency Level:      10
Time taken for tests:   0.973 seconds
Complete requests:      1000
Failed requests:        0
Total transferred:      462000 bytes
HTML transferred:       323000 bytes
Requests per second:    1027.72 [#/sec] (mean)
Time per request:       9.730 [ms] (mean)
Time per request:       0.973 [ms] (mean, across all concurrent requests)
Transfer rate:          463.68 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.0      0       0
Processing:     3   10   3.5      8      26
Waiting:        3    9   3.5      8      26
Total:          3   10   3.5      8      26

Percentage of the requests served within a certain time (ms)
  50%      8
  66%      9
  75%     10
  80%     10
  90%     14
  95%     19
  98%     21
  99%     24
 100%     26 (longest request)

```
 
### n=1000, c=100

```
sudo ab -n 1000 -c 100 http://127.0.0.1/api/v1/account/
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking 127.0.0.1 (be patient)
Completed 100 requests
Completed 200 requests
Completed 300 requests
Completed 400 requests
Completed 500 requests
Completed 600 requests
Completed 700 requests
Completed 800 requests
Completed 900 requests
Completed 1000 requests
Finished 1000 requests


Server Software:        Kestrel
Server Hostname:        127.0.0.1
Server Port:            80

Document Path:          /api/v1/account/
Document Length:        323 bytes

Concurrency Level:      100
Time taken for tests:   1.088 seconds
Complete requests:      1000
Failed requests:        0
Total transferred:      462000 bytes
HTML transferred:       323000 bytes
Requests per second:    919.25 [#/sec] (mean)
Time per request:       108.785 [ms] (mean)
Time per request:       1.088 [ms] (mean, across all concurrent requests)
Transfer rate:          414.74 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.9      0       4
Processing:    11  101  29.4     89     205
Waiting:       11  101  29.4     89     205
Total:         15  102  29.7     89     206

Percentage of the requests served within a certain time (ms)
  50%     89
  66%     96
  75%    109
  80%    119
  90%    146
  95%    172
  98%    193
  99%    201
 100%    206 (longest request)
```
