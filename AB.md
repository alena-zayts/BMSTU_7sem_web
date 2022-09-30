## С балансировкой
### 100

```
sudo ab -n 100 -c 10 http://127.0.0.1/api/v1/lifts/A0
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking 127.0.0.1 (be patient).....done


Server Software:        Kestrel
Server Hostname:        127.0.0.1
Server Port:            80

Document Path:          /api/v1/lifts/A0
Document Length:        1405 bytes

Concurrency Level:      10
Time taken for tests:   10.328 seconds
Complete requests:      100
Failed requests:        0
Total transferred:      156000 bytes
HTML transferred:       140500 bytes
Requests per second:    9.68 [#/sec] (mean)
Time per request:       1032.762 [ms] (mean)
Time per request:       103.276 [ms] (mean, across all concurrent requests)
Transfer rate:          14.75 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.1      0       1
Processing:   649  946  80.5    946    1142
Waiting:      649  946  80.5    945    1142
Total:        649  947  80.5    946    1142

Percentage of the requests served within a certain time (ms)
  50%    946
  66%    971
  75%    987
  80%    999
  90%   1064
  95%   1085
  98%   1110
  99%   1142
 100%   1142 (longest request)
```
