using StableIndexCollection;
using System.Diagnostics;

const int ITEM_COUNT = 10_000_000;
const int REMOVE_COUNT = 1_000;

var intItems = new int[ITEM_COUNT];
var callableItems = new CallableFunction[ITEM_COUNT];

for (var i = 0; i < 1_000_000; i++)
{
    intItems[i] = i;
    callableItems[i] = new CallableFunction();
}

LinkedList<int> linkedListInt = new();
List<int> listInt = [];
StableIndexCollection<int> sivInt = [];

int sum = 0;

Console.WriteLine(sum);
sum = 0;
var random = new Random();
var seed = random.Next();


random = new Random(seed);
BenchmarkFunction(() => linkedListInt = new LinkedList<int>(intItems), "create LinkedList<int>");
BenchmarkFunction(() =>
{
    foreach(var item in linkedListInt)
    {
        sum = unchecked(sum + item);
    }
}, "sum LinkedList<int>");
BenchmarkFunction(() =>
{
    for(var i = 0; i < REMOVE_COUNT; i++)
    {
        linkedListInt.Remove(i);
    }
}, "remove LinkedList<int>");
BenchmarkFunction(() =>
{
    for (var i = 0; i < REMOVE_COUNT; i++)
    {
        linkedListInt.AddLast(i);
    }
}, "add LinkedList<int>");

Console.WriteLine(sum);
sum = 0;

linkedListInt.Clear();

random = new Random(seed);
BenchmarkFunction(() => listInt = [.. intItems], "create List<int>");
BenchmarkFunction(() =>
{
    foreach(var item in listInt)
    {
        sum = unchecked(sum + item);
    }
}, "sum List<int>");
BenchmarkFunction(() =>
{
    for (var i = 0; i < REMOVE_COUNT; i++)
    {
        listInt.Remove(i);
    }
}, "remove List<int>");
BenchmarkFunction(() =>
{
    for (var i = 0; i < REMOVE_COUNT; i++)
    {
        listInt.Add(i);
    }
}, "add List<int>");

listInt.Clear();

Console.WriteLine(sum);
sum = 0;

random = new Random(seed);
BenchmarkFunction(() => sivInt = [.. intItems], "create StableIndexCollection<int>");
BenchmarkFunction(() =>
{
    foreach(var item in sivInt)
    {
        sum = unchecked(sum + item);
    }
}, "sum StableIndexCollection<int>");
BenchmarkFunction(() =>
{
    for (var i = 0; i < REMOVE_COUNT; i++)
    {
        sivInt.Remove(i);
    }
}, "remove StableIndexCollection<int>");
BenchmarkFunction(() =>
{
    for (var i = 0; i < REMOVE_COUNT; i++)
    {
        sivInt.Add(i);
    }
}, "add StableIndexCollection<int>");

Console.WriteLine(sum);
sum = 0;

static void BenchmarkFunction(Action action, string name)
{
    var stopWatch = Stopwatch.StartNew();
    action();
    stopWatch.Stop();
    Console.WriteLine($"{name}: {stopWatch.ElapsedMilliseconds} ms");
}

class CallableFunction
{
    private int _value = 0;

    public void Update()
    {
        _value++;
    }
}