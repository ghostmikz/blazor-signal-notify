namespace ServerCounterApp.Client.Services;

using Microsoft.JSInterop;
using Microsoft.Extensions.Configuration;

public class PushNotificationService(IJSRuntime jsRuntime, IConfiguration config)
{
    public async Task<object> SubscribeUserAsync()
    {
        var publicKey = config["Vapid:PublicKey"];
        return await jsRuntime.InvokeAsync<object>(
            "blazorPushNotifications.requestSubscription", publicKey);
    }

    // This is the 2nd one:
    public async Task SetUnreadCount(int count)
    {
        await jsRuntime.InvokeVoidAsync("blazorPushNotifications.updateTabTitle", count);
    }
}