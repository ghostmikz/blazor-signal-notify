using Microsoft.AspNetCore.Mvc;
using Lib.Net.Http.WebPush;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/notifications")] // <-- THE URL BASE
public class PushController : ControllerBase
{
    private readonly PushServiceClient _pushClient;

    public PushController(PushServiceClient pushClient)
    {
        _pushClient = pushClient;
    }

    [HttpPost("subscribe/{username}/{role}")]
    public IActionResult Subscribe(string username, string role, [FromBody] PushSubscription subscription)
    {
        // Force the key to be consistent
        SubscriptionStore.AddSubscription(role, subscription);
        Console.WriteLine($">>> [SERVER] Subscribed: {username} as {role}");
        return Ok();
    }

    [HttpPost("request-leave")]
    public async Task<IActionResult> RequestLeave()
    {
        string msgText = $"Leave Request at {DateTime.Now:T}";
        SubscriptionStore.LogRequest(msgText);

        // We look specifically for "Manager"
        if (SubscriptionStore.TryGetSubscription("Manager", out var sub))
        {
            try {
                var message = new PushMessage(msgText) { Urgency = PushMessageUrgency.High };
                await _pushClient.RequestPushMessageDeliveryAsync(sub, message);
                Console.WriteLine(">>> [SERVER] Push sent to Manager.");
            } catch (Exception ex) { 
                Console.WriteLine($">>> [SERVER] Push failed: {ex.Message}");
            }
        }
        return Ok();
    }

    [HttpGet("history")]
    public IActionResult GetHistory() => Ok(SubscriptionStore.GetLogs());

    [HttpPost("unsubscribe/{role}")]
    public IActionResult Unsubscribe(string role)
    {
        // This is the "Wire Cutter"
        SubscriptionStore.RemoveSubscription(role);
        Console.WriteLine($">>> [SERVER] SUCCESS: {role} is now UNSUBSCRIBED.");
        return Ok();
    }
}