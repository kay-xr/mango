using Mango.Server.Models;
using Newtonsoft.Json;
using Terminal.Gui;
using ConfigurationManager = Terminal.Gui.ConfigurationManager;

namespace Mango.Server;

internal static class Program
{
    private static bool _useTui = true;
    private static Task _apiTask;

    public static ServerManager ServerManager { get; } = new();
    public static MangoConfig? MangoConfig { get; set; }

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Initializing Mango...");
        
        // Load config
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();

        if (!File.Exists("./mango-config.json"))
        {
            await RunSetup();
        }
        else
        {
            var configFile = await File.ReadAllTextAsync("./mango-config.json");
            MangoConfig = JsonConvert.DeserializeObject<MangoConfig>(configFile);
        }

        // Build the api
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        var host = hostBuilder.Build();
        _apiTask = host.RunAsync();

        foreach (var argument in args)
            if (argument.Contains("--no-tui") || argument.Contains("-n"))
                _useTui = false;

        if (_useTui) RunTui();

        // Wait for api
        await _apiTask;
    }

    private static void RunTui()
    {
        var tui = new Tui(ServerManager);
        ConfigurationManager.RuntimeConfig = """{ "Theme": "Dark" }""";
        Application.Init();
        Application.Run(tui);
        Application.Shutdown();
    }

    private static async Task RunSetup()
    {
        
    }
}