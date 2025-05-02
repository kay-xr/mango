namespace Mango.Host.Models;

public class MangoConfig
{
    public int ApiPort = 6609;
    public string ServerType = "";
    public string ServerDirectory = "";
    public string ServerExecutable = "";
    public string ServerArguments = "";
    public int? SteamAppId = null;
    public bool UseHttps = true;
    public string? CertFilePath = null;
    public string? CertFilePassword = null;
}