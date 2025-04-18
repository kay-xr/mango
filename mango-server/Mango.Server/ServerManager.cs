using System.Diagnostics;

namespace Mango.Server;

public class ServerManager
{
    private Process? _process;

    // TODO: Make these configurable as needed.
    private const string ExecutablePath = "./test/Mango.TestApplication";
    private const string DefaultArguments = "";

    public bool IsRunning => _process != null && !_process.HasExited;

    public event Action<string>? OutputDataReceived;

    /// <summary>
    ///     Start the server if it's not running
    /// </summary>
    public async Task StartServerAsync()
    {
        if (IsRunning)
        {
            Console.WriteLine("Server is already running!");
            return;
        }

        var psi = new ProcessStartInfo
        {
            FileName = ExecutablePath,
            Arguments = DefaultArguments,
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
                        // Console.WriteLine($"[Server Output] {line}");
                        OutputDataReceived?.Invoke($"[Server Output] {line}");
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

    /// <summary>
    ///     Stop the server if running
    /// </summary>
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

    /// <summary>
    ///     Restart server by stopping, waiting 10s and restarting
    /// </summary>
    public async Task RestartServerAsync()
    {
        await StopServerAsync();
        await Task.Delay(10000);
        await StartServerAsync();
    }
}