using System.Reflection;

class Program
{
    static async Task Main(string[] args)
    {
        var cts = new CancellationTokenSource();

        Console.WriteLine("Press 'C' to cancel the operation.");

        // Monitor for cancellation via key press
        _ = Task.Run(() =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                if (char.ToUpper(Console.ReadKey().KeyChar) == 'C')
                    cts.Cancel();
            }
        });

        try
        {
            var filePath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory,
                @"largefile.txt"
            );
            await ProcessLargeFileAsync(filePath, cts.Token);
            Console.WriteLine("File processing completed.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("File processing was canceled.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            cts.Dispose();
        }
    }

    static async Task ProcessLargeFileAsync(string filePath, CancellationToken token)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        using (var reader = new StreamReader(filePath))
        {
            string? line;
            int lineCount = 0;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                token.ThrowIfCancellationRequested(); // Check for cancellation

                // Simulate line processing
                await Task.Delay(1000); // Simulate I/O delay
                Console.WriteLine($"Processing line {++lineCount}: {line}");
            }
        }
    }
}