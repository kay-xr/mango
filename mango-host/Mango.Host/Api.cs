namespace Mango.Host;

public class Api
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("ping", () => "Pong!");

            endpoints.MapPost("start", async () =>
            {
                if (Program.ServerManager != null)
                {
                    await Program.ServerManager.StartServerAsync();
                }
                else
                {
                    Console.WriteLine("Error!");
                    Results.InternalServerError("Something went wrong: Server manager could not be accessed!");
                    throw new Exception("Server Manager was not found!");
                }
            });
            
            endpoints.MapPost("stop", async () =>
            {
                if (Program.ServerManager != null)
                {
                    await Program.ServerManager.StopServerAsync();
                }
                else
                {
                    Console.WriteLine("Error!");
                    Results.InternalServerError("Something went wrong: Server manager could not be accessed!");
                    throw new Exception("Server Manager was not found!");
                }
            });
            
            endpoints.MapPost("restart", async () =>
            {
                if (Program.ServerManager != null)
                {
                    await Program.ServerManager.RestartServerAsync();
                }
                else
                {
                    Console.WriteLine("Error!");
                    Results.InternalServerError("Something went wrong: Server manager could not be accessed!");
                    throw new Exception("Server Manager was not found!");
                }
            });
        });
    }
}