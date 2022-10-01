-n – количество запросов, которые нужно выполнить для сеанса бенчмаркинга.

-c – это параллелизм, который обозначает количество одновременных запросов. 

# Без балансировкм

## n=10000, c=100
```
sudo ab -n 10000 -c 100 http://127.0.0.1/api/v1/account/
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking 127.0.0.1 (be patient)
Completed 1000 requests
Completed 2000 requests
Completed 3000 requests
Completed 4000 requests
Completed 5000 requests
Completed 6000 requests
Completed 7000 requests
Completed 8000 requests
Completed 9000 requests
Completed 10000 requests
Finished 10000 requests


Server Software:        Kestrel
Server Hostname:        127.0.0.1
Server Port:            80

Document Path:          /api/v1/account/
Document Length:        323 bytes

Concurrency Level:      100
Time taken for tests:   9.051 seconds
Complete requests:      10000
Failed requests:        0
Total transferred:      4620000 bytes
HTML transferred:       3230000 bytes
Requests per second:    1104.87 [#/sec] (mean)
Time per request:       90.509 [ms] (mean)
Time per request:       0.905 [ms] (mean, across all concurrent requests)
Transfer rate:          498.48 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.4      0       5
Processing:     2   90  10.6     87     175
Waiting:        2   90  10.6     87     175
Total:          2   90  10.7     87     179

Percentage of the requests served within a certain time (ms)
  50%     87
  66%     90
  75%     94
  80%     97
  90%    103
  95%    108
  98%    115
  99%    126
 100%    179 (longest request)
 ```


## n=100000, c=1000
```
sudo ab -n 100000 -c 1000 http://127.0.0.1/api/v1/account/
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking 127.0.0.1 (be patient)
Completed 10000 requests
Completed 20000 requests
Completed 30000 requests
Completed 40000 requests
Completed 50000 requests
Completed 60000 requests
Completed 70000 requests
Completed 80000 requests
Completed 90000 requests
Completed 100000 requests
Finished 100000 requests


Server Software:        Kestrel
Server Hostname:        127.0.0.1
Server Port:            80

Document Path:          /api/v1/account/
Document Length:        323 bytes

Concurrency Level:      1000
Time taken for tests:   16.286 seconds
Complete requests:      100000
Failed requests:        85302
   (Connect: 0, Receive: 0, Length: 85302, Exceptions: 0)
Non-2xx responses:      85302
Total transferred:      34685190 bytes
HTML transferred:       18908226 bytes
Requests per second:    6140.21 [#/sec] (mean)
Time per request:       162.861 [ms] (mean)
Time per request:       0.163 [ms] (mean, across all concurrent requests)
Transfer rate:          2079.83 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0   52  16.0     50     131
Processing:    11  108  80.1     78    1341
Waiting:        2   88  79.8     60    1323
Total:         39  160  83.9    129    1362

Percentage of the requests served within a certain time (ms)
  50%    129
  66%    146
  75%    164
  80%    185
  90%    309
  95%    350
  98%    400
  99%    428
 100%   1362 (longest request)
 
 ```
 
 

 
 
 
 
## С балансировкой
### n=10000, c=100
```
sudo ab -n 10000 -c 100 http://127.0.0.1/api/v1/account/
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking 127.0.0.1 (be patient)
Completed 1000 requests
Completed 2000 requests
Completed 3000 requests
Completed 4000 requests
Completed 5000 requests
Completed 6000 requests
Completed 7000 requests
Completed 8000 requests
Completed 9000 requests
Completed 10000 requests
Finished 10000 requests


Server Software:        Kestrel
Server Hostname:        127.0.0.1
Server Port:            80

Document Path:          /api/v1/account/
Document Length:        323 bytes

Concurrency Level:      100
Time taken for tests:   7.450 seconds
Complete requests:      10000
Failed requests:        0
Total transferred:      4620000 bytes
HTML transferred:       3230000 bytes
Requests per second:    1342.31 [#/sec] (mean)
Time per request:       74.499 [ms] (mean)
Time per request:       0.745 [ms] (mean, across all concurrent requests)
Transfer rate:          605.61 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.5      0      13
Processing:     2   74  68.9     76     263
Waiting:        2   74  68.9     76     263
Total:          2   74  69.0     77     265

Percentage of the requests served within a certain time (ms)
  50%     77
  66%    137
  75%    140
  80%    142
  90%    149
  95%    156
  98%    166
  99%    175
 100%    265 (longest request)
 ```
 
 ### n=100000, c=1000
```
sudo ab -n 100000 -c 1000 http://127.0.0.1/api/v1/account/
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking 127.0.0.1 (be patient)
Completed 10000 requests
Completed 20000 requests
Completed 30000 requests
Completed 40000 requests
Completed 50000 requests
Completed 60000 requests
Completed 70000 requests
Completed 80000 requests
Completed 90000 requests
Completed 100000 requests
Finished 100000 requests


Server Software:        Kestrel
Server Hostname:        127.0.0.1
Server Port:            80

Document Path:          /api/v1/account/
Document Length:        323 bytes

Concurrency Level:      1000
Time taken for tests:   7.558 seconds
Complete requests:      100000
Failed requests:        99364
   (Connect: 0, Receive: 0, Length: 99364, Exceptions: 0)
Non-2xx responses:      99364
Total transferred:      32796840 bytes
HTML transferred:       16707172 bytes
Requests per second:    13230.33 [#/sec] (mean)
Time per request:       75.584 [ms] (mean)
Time per request:       0.076 [ms] (mean, across all concurrent requests)
Transfer rate:          4237.43 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0   31   7.4     30      68
Processing:     6   44  36.8     40     721
Waiting:        4   33  36.5     28     714
Total:         10   75  37.0     70     754

Percentage of the requests served within a certain time (ms)
  50%     70
  66%     74
  75%     76
  80%     77
  90%     84
  95%     94
  98%    119
  99%    132
 100%    754 (longest request)
```
 
