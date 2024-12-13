class Program
{
    static object lockObject = new object();
    static void Main(string[] args)
    {
        long IncrementValue = 0;
        long SumValue = 0;

        Parallel.For(0, 10000, number =>
        {
            //Before lock Parallel 
            lock (lockObject)
            {
                IncrementValue++;
                SumValue += IncrementValue;
            }
            //After lock Parallel 
        });

        Console.WriteLine($"Increment Value With lock: {IncrementValue}");
        Console.WriteLine($"Sum Value With lock: {SumValue}");
        Console.ReadKey();
    }
}