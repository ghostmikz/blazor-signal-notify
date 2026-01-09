using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;

namespace Server.Services;

public class CounterWorker : BackgroundService
{
    private readonly IHubContext<CounterHub> _hubContext;
    private readonly PushServiceClient _webPushClient = new();
    private int _count = 0;

    private const string VapidPublic = "BKMxMXsdv11hILIHp31vatUVgIKu_Pl57-nNnkHii8I1hPcC0dph7UyPeNEqRddCD9cbFpqpMxQLK4GGwRmT-nw";
    private const string VapidPrivate = "CwYDxVev4qZIbtdC54vi_LDH4QJLCUHuNvjUvs6Ny_o";

    public static PushSubscription? CurrentSubscription { get; set; }

    public CounterWorker(IHubContext<CounterHub> hubContext)
    {
        _hubContext = hubContext;
        _webPushClient.DefaultAuthentication = new VapidAuthentication(VapidPublic, VapidPrivate)
        {
            Subject = "mailto:example@example.com"
        };
    }
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        _count++;
        await _hubContext.Clients.All.SendAsync("ReceiveCount", _count, stoppingToken);

        if (_count % 10 == 0)
        {
            var sub = SubscriptionStore.Current;

            Console.WriteLine($"[Worker] Count: {_count} | Sub Status: {(sub == null ? "EMPTY" : "READY")}");

            if (sub != null)
            {
                try {
                    var message = new PushMessage($"Milestone: {_count}");
                    await _webPushClient.RequestPushMessageDeliveryAsync(sub, message, stoppingToken);
                    Console.WriteLine(">>> PUSH SENT SUCCESSFULLY <<<");
                } catch (Exception ex) {
                    Console.WriteLine($">>> PUSH SEND ERROR: {ex.Message}");
                }
            }
        }
        await Task.Delay(1000, stoppingToken);
    }
}
}