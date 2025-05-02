using System.Diagnostics;
using Mango.Host.Models;

namespace Mango.Host;

public class ServerManager
{
    private MangoConfig _mangoConfig;
    private Process? _process;
    public bool IsRunning => _process != null && !_process.HasExited;
    public event Action<string>? OutputDataReceived;

    public ServerManager(MangoConfig mangoConfig)
    {
        _mangoConfig = mangoConfig;
    }
    
    public async Task StartServerAsync()
    {
        if (IsRunning)
        {
            Console.WriteLine("Server is already running!");
            return;
        }
        
        var psi = new ProcessStartInfo
        {
            FileName = _mangoConfig.ServerExecutable,
            Arguments = _mangoConfig.ServerArguments,
            WorkingDirectory = _mangoConfig.ServerDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _process = new Process { StartInfo = psi, EnableRaisingEvents = true };
        _process.Exited += (sender, e) =>
        {
            Console.WriteLine("Server process exited.");
            OutputDataReceived?.Invoke("Server process exited.");
        };

        if (_process.Start())
        {
            Console.WriteLine("Server process started.");
            OutputDataReceived?.Invoke("Server process started.");

            _ = Task.Run(() =>
            {
                while (!_process.StandardOutput.EndOfStream)
                {
                    var line = _process.StandardOutput.ReadLine();
                    if (line is not null)
                    {
                        Console.WriteLine($"[Server Output] {line}");
                        OutputDataReceived?.Invoke($"[Server Output] {line}");
                    }
                }
            });

            _ = Task.Run(() =>
            {
                while (!_process.StandardError.EndOfStream)
                {
                    var line = _process.StandardError.ReadLine();
                    if (line is not null)
                    {
                        Console.WriteLine($"[Server Error] {line}");
                        OutputDataReceived?.Invoke($"[Server Error] {line}");
                    }
                }
            });
        }
    }
    
    public async Task StopServerAsync()
    {
        if (!IsRunning)
        {
            Console.WriteLine("Server is not running!");
            OutputDataReceived?.Invoke("Server is not running!");
            return;
        }

        try
        {
            _process.Kill();
            await _process.WaitForExitAsync();
            Console.WriteLine("Server process stopped.");
            OutputDataReceived?.Invoke("Server process stopped.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to stop server process: {ex.Message}");
            OutputDataReceived?.Invoke($"Failed to stop server process: {ex.Message}");
        }
    }
    
    public async Task RestartServerAsync()
    {
        await StopServerAsync();
        Console.WriteLine("Server process exited, waiting 10s...");
        await Task.Delay(10000);
        await StartServerAsync();
    }
}