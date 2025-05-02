using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Mango.Host.Models;
using Mango.Host.Utils;
using Newtonsoft.Json;

namespace Mango.Host;

public class Setup
{
    public async Task RunSetup()
    {
        Console.WriteLine("Config file not found, running Mango setup...");
        var config = new MangoConfig();

        config = await InitializeHttps(config);

        var configWriter = File.CreateText("./mango-config.json");
        await configWriter.WriteAsync(JsonConvert.SerializeObject(config, Formatting.Indented));
        configWriter.Close();
    }

    /// <summary>
    ///     Because we are running these hosts in a way which should only directly communicate with our master API, we
    ///     are generally okay using localhost certificates because we just want to add a tiny bit of encryption to protect
    ///     our keys. As such, I have decided for now to auto-generate a certificate as localhost on the machine for this
    ///     connection, even though our server will ignore it. Users will still have the option to disable https or use
    ///     their own proper certificates in the configuration files.
    ///     This is the most user-friendly way I can think of doing this, even though its honestly not super necessary.
    /// </summary>
    private async Task<MangoConfig> InitializeHttps(MangoConfig mangoConfig)
    {
        if (!Directory.Exists("./certs")) Directory.CreateDirectory("./certs");

        var ecdsa = ECDsa.Create();
        var certReq = new CertificateRequest("CN=localhost", ecdsa, HashAlgorithmName.SHA256);
        var cert = certReq.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(10));

        var autogenPass = RandomString.Generate();

        var pfxBytes = cert.Export(X509ContentType.Pfx, autogenPass);
        await File.WriteAllBytesAsync("./certs/generated.pfx", pfxBytes);

        mangoConfig.CertFilePath = "./certs/generated.pfx";
        mangoConfig.CertFilePassword = autogenPass;

        return mangoConfig;
    }

    public async Task<MangoConfig> ValidateConfig(MangoConfig mangoConfig)
    {
        if (mangoConfig.ApiPort is <= 1 or > 65535)
        {
            throw new Exception("Config Error: API port must be between 1-65535");
        }

        if (string.IsNullOrWhiteSpace(mangoConfig.ServerType))
        {
            throw new Exception(
                "Config Error: Mango host must have a configuration type!\nPossible options are:\n\"minecraft\"\n\"steam\"\n\"generic\"");
        }

        if (string.IsNullOrWhiteSpace(mangoConfig.ServerDirectory))
        {
            throw new Exception(
                "Config Error: No server directory was specified! Please set the server's directory in the configuration file.\n" +
                "Examples:\n" +
                "../server/server-executable (relative)\n" +
                "/home/user/server/server-executable (exact)\n" +
                "C:\\\\Users\\\\User\\\\Server\\\\Server.exe (windows)");
        }

        if (string.IsNullOrWhiteSpace(mangoConfig.ServerExecutable))
        {
            throw new Exception(
                "Config Error: Mango host must have a server executable to run! Please set the path in the configuration file.\n" +
                "Examples:\n" +
                "../server/ (relative)\n" +
                "/home/user/server/ (exact)\n" +
                "C:\\\\Users\\\\User\\\\Server\\ (windows)");
        }

        if (mangoConfig.UseHttps && string.IsNullOrWhiteSpace(mangoConfig.CertFilePath))
        {
            Console.WriteLine("HTTPS was enabled but no certificate was specified, generating one now!");
            mangoConfig = await InitializeHttps(mangoConfig);
        }

        if (!mangoConfig.UseHttps)
        {
            Console.WriteLine("WARNING: HTTPS is disabled for this host, this is insecure, and not recommended!");
        }

        if ((mangoConfig.ServerType == "steam" && mangoConfig.SteamAppId == null) ||
            (mangoConfig.ServerType == "steam" && mangoConfig.SteamAppId <= 1))
        {
            Console.Write(
                "Config: Server was set to type \"steam\", however no app id was specified, disabling updater.");
        }

        return mangoConfig;
    }
}