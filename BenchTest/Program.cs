using StableIndexCollection;
using System.Diagnostics;

int ITEM_COUNT_SEED = 1_000;
int REMOVE_COUNT_SEED = 10;

for (var l = 1; l < 5; l++)
{
    var ITEM_COUNT = ITEM_COUNT_SEED * (int)Math.Pow(10, l);
    var REMOVE_COUNT = REMOVE_COUNT_SEED * (int)Math.Pow(10, l);

    var intItems = new int[ITEM_COUNT];

    for (var i = 0; i < ITEM_COUNT; i++)
    {
        intItems[i] = i;
    }

    LinkedList<int> linkedListInt = new();
    List<int> listInt = [];
    StableIndexCollection<int> sivInt = [];

    int sum = 0;

    Console.WriteLine($"{nameof(ITEM_COUNT)}: {ITEM_COUNT}, {nameof(REMOVE_COUNT)}: {REMOVE_COUNT}");
    sum = 0;
    var random = new Random();
    var seed = random.Next();

    random = new Random(seed);
    BenchmarkFunction(() => linkedListInt = new LinkedList<int>(intItems), "create LinkedList<int>");
    BenchmarkFunction(() =>
    {
        foreach (var item in linkedListInt)
        {
            sum = unchecked(sum + item);
        }
    }, "sum LinkedList<int>");
    BenchmarkFunction(() =>
    {
        for (var i = 0; i < REMOVE_COUNT; i++)
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
        foreach (var item in listInt)
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
        foreach (var item in sivInt)
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
}

static void BenchmarkFunction(Action action, string name)
{
    var stopWatch = Stopwatch.StartNew();
    action();
    stopWatch.Stop();
    Console.WriteLine($"{name}: {stopWatch.ElapsedMilliseconds} ms");
}