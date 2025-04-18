namespace Mango.Server;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/ping", async context => { await context.Response.WriteAsync("Pong!"); });
            
            endpoints.MapGet("/", async context => { await context.Response.WriteAsJsonAsync("Mango Server is online!"); });

            endpoints.MapGet("/start", async context =>
            {
                await Program.ServerManager.StartServerAsync();
                await context.Response.WriteAsync("Server started.");
            });

            endpoints.MapGet("/stop", async context =>
            {
                await Program.ServerManager.StopServerAsync();
                await context.Response.WriteAsync("Server stopped.");
            });

            endpoints.MapGet("/restart", async context =>
            {
                await Program.ServerManager.RestartServerAsync();
                await context.Response.WriteAsync("Server restarted.");
            });
        });
    }
}