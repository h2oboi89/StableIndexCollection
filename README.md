# C# Implementation of https://github.com/johnBuffer/StableIndexVector

From https://youtu.be/L4xOCvELWlU?si=XLfA5Pe0euxiIB2W

## Bench Test Output
```
> .\BenchTest.exe
ITEM_COUNT: 10000, REMOVE_COUNT: 100
create LinkedList<int>: 2 ms
sum LinkedList<int>: 0 ms
remove LinkedList<int>: 2 ms
add LinkedList<int>: 0 ms
49995000
create List<int>: 0 ms
sum List<int>: 0 ms
remove List<int>: 0 ms
add List<int>: 0 ms
49995000
create StableIndexCollection<int>: 2 ms
sum StableIndexCollection<int>: 0 ms
remove StableIndexCollection<int>: 0 ms
add StableIndexCollection<int>: 0 ms
49995000
ITEM_COUNT: 100000, REMOVE_COUNT: 1000
create LinkedList<int>: 4 ms
sum LinkedList<int>: 0 ms
remove LinkedList<int>: 59 ms
add LinkedList<int>: 0 ms
704982704
create List<int>: 0 ms
sum List<int>: 0 ms
remove List<int>: 8 ms
add List<int>: 0 ms
704982704
create StableIndexCollection<int>: 8 ms
sum StableIndexCollection<int>: 0 ms
remove StableIndexCollection<int>: 0 ms
add StableIndexCollection<int>: 0 ms
704982704
ITEM_COUNT: 1000000, REMOVE_COUNT: 10000
create LinkedList<int>: 87 ms
sum LinkedList<int>: 3 ms
remove LinkedList<int>: 16466 ms
add LinkedList<int>: 1 ms
1783293664
create List<int>: 0 ms
sum List<int>: 0 ms
remove List<int>: 2891 ms
add List<int>: 1 ms
1783293664
create StableIndexCollection<int>: 258 ms
sum StableIndexCollection<int>: 1 ms
remove StableIndexCollection<int>: 10 ms
add StableIndexCollection<int>: 3 ms
1783293664
ITEM_COUNT: 10000000, REMOVE_COUNT: 100000
create LinkedList<int>: 1078 ms
sum LinkedList<int>: 39 ms
remove LinkedList<int>: 1754789 ms
add LinkedList<int>: 3 ms
-2014260032
create List<int>: 11 ms
sum List<int>: 5 ms
remove List<int>: 321707 ms
add List<int>: 13 ms
-2014260032
create StableIndexCollection<int>: 2400 ms
sum StableIndexCollection<int>: 14 ms
remove StableIndexCollection<int>: 218 ms
add StableIndexCollection<int>: 23 ms
-2014260032
```
