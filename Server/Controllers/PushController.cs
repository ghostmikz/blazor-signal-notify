using Microsoft.AspNetCore.Mvc;
using Lib.Net.Http.WebPush; // Ensure this is here
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/notifications")]
public class PushController : ControllerBase
{
[HttpPost("subscribe")]
public IActionResult Subscribe([FromBody] System.Text.Json.JsonElement rawJson)
{
    // Log the RAW JSON string to the terminal
    Console.WriteLine("SERVER RECEIVED RAW DATA: " + rawJson.ToString());

    try {
        var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var subscription = System.Text.Json.JsonSerializer.Deserialize<Lib.Net.Http.WebPush.PushSubscription>(rawJson.GetRawText(), options);

        if (subscription != null && !string.IsNullOrEmpty(subscription.Endpoint)) {
            Server.Services.SubscriptionStore.Current = subscription;
            Console.WriteLine("===> SUCCESS: Subscription Saved! <===");
            return Ok();
        }
    } catch (Exception ex) {
        Console.WriteLine("SERVER ERROR: " + ex.Message);
    }

    return BadRequest("Data was invalid");
}
}