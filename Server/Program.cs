using Lib.Net.Http.WebPush;
using Server.Hubs;
using Server.Services;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

// 1. Register Web Push Service
builder.Services.AddHttpClient<PushServiceClient>();
builder.Services.AddSingleton<PushServiceClient>(sp => 
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var client = new PushServiceClient(httpClient);
    
    client.DefaultAuthentication = new Lib.Net.Http.WebPush.Authentication.VapidAuthentication(
        "BKMxMXsdv11hILIHp31vatUVgIKu_Pl57-nNnkHii8I1hPcC0dph7UyPeNEqRddCD9cbFpqpMxQLK4GGwRmT-nw", 
        "CwYDxVev4qZIbtdC54vi_LDH4QJLCUHuNvjUvs6Ny_o")
    {
        Subject = "mailto:admin@example.com"
    };
    return client;
});

// 2. Add a Silent Heartbeat to prevent the 1-minute delay
// This replaces the CounterWorker but sends NO data to the UI
builder.Services.AddHostedService<HeartbeatWorker>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowClient", policy => {
        policy.WithOrigins("http://localhost:5184", "http://127.0.0.1:5184") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowClient"); 

app.MapControllers();
app.MapHub<CounterHub>("/counterhub");

app.Run();

// --- SILENT WORKER DEFINITION ---
public class HeartbeatWorker(IHubContext<CounterHub> hubContext) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Just keeps the pipe warm, sends no visible count
            await hubContext.Clients.All.SendAsync("Ping", stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}