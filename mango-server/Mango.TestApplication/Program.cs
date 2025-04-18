namespace Mango.TestApplication;

class Program
{
    static async Task Main(string[] args)
    {
        await Task.Delay(1);

        while (true)
        {
            Console.WriteLine("Hello World!");
            await Task.Delay(1000);
        }
    }
}