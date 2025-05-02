using Mango.Host.Models;
using Newtonsoft.Json;

namespace Mango.Host;

internal static class Program
{
    private static Task? _apiTask;
    public static MangoConfig? MangoConfig;
    public static ServerManager? ServerManager { get; set; }
    
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Initializing Mango...");

        // Configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var mangoSetup = new Setup();
        
        if (!File.Exists("./mango-config.json"))
        {
            await mangoSetup.RunSetup();
        }
        
        var configFile = await File.ReadAllTextAsync("./mango-config.json");
        MangoConfig = JsonConvert.DeserializeObject<MangoConfig>(configFile);

        if (MangoConfig == null) throw new NullReferenceException("mango-config.json not found! Cannot run Mango!");
        
        MangoConfig = await mangoSetup.ValidateConfig(MangoConfig);

        ServerManager = new ServerManager(MangoConfig);
        
        // API Setup
        var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    var port = MangoConfig.ApiPort;
                    if (MangoConfig.UseHttps)
                    {
                        if (!string.IsNullOrWhiteSpace(MangoConfig.CertFilePath) && File.Exists(MangoConfig.CertFilePath))
                        {
                            serverOptions.ListenAnyIP(port, listenOptions =>
                            {
                                listenOptions.UseHttps(MangoConfig.CertFilePath, MangoConfig.CertFilePassword);
                            });
                        }
                        else
                        {
                            Console.WriteLine("Config Error: HTTPS is enabled but no certificate was provided or found. Falling back to HTTP.");
                            serverOptions.ListenAnyIP(port);
                        }
                    }
                    else
                    {
                        serverOptions.ListenAnyIP(port);
                    }
                });
                
                webBuilder.UseStartup<Api>();
            });

        var host = hostBuilder.Build();
        _apiTask = host.RunAsync();

        await _apiTask;
    }
}