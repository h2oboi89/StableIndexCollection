# C# Implementation of https://github.com/johnBuffer/StableIndexVector

## Bench Test Output
```
> .\BenchTest.exe
ITEM_COUNT: 10000, REMOVE_COUNT: 100
create LinkedList<int>: 1 ms
sum LinkedList<int>: 0 ms
remove LinkedList<int>: 0 ms
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
create LinkedList<int>: 2 ms
sum LinkedList<int>: 0 ms
remove LinkedList<int>: 0 ms
add LinkedList<int>: 0 ms
704982704
create List<int>: 0 ms
sum List<int>: 0 ms
remove List<int>: 4 ms
add List<int>: 0 ms
704982704
create StableIndexCollection<int>: 11 ms
sum StableIndexCollection<int>: 0 ms
remove StableIndexCollection<int>: 0 ms
add StableIndexCollection<int>: 0 ms
704982704
ITEM_COUNT: 1000000, REMOVE_COUNT: 10000
create LinkedList<int>: 72 ms
sum LinkedList<int>: 3 ms
remove LinkedList<int>: 0 ms
add LinkedList<int>: 0 ms
1783293664
create List<int>: 0 ms
sum List<int>: 0 ms
remove List<int>: 3922 ms
add List<int>: 0 ms
1783293664
create StableIndexCollection<int>: 201 ms
sum StableIndexCollection<int>: 1 ms
remove StableIndexCollection<int>: 6 ms
add StableIndexCollection<int>: 1 ms
1783293664
ITEM_COUNT: 10000000, REMOVE_COUNT: 100000
create LinkedList<int>: 817 ms
sum LinkedList<int>: 28 ms
remove LinkedList<int>: 0 ms
add LinkedList<int>: 2 ms
-2014260032
create List<int>: 6 ms
sum List<int>: 5 ms
remove List<int>: 483146 ms
add List<int>: 0 ms
-2014260032
create StableIndexCollection<int>: 2792 ms
sum StableIndexCollection<int>: 14 ms
remove StableIndexCollection<int>: 403 ms
add StableIndexCollection<int>: 1 ms
-2014260032
```