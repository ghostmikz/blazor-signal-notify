using Lib.Net.Http.WebPush;

namespace Server.Services;

public static class SubscriptionStore
{
    public static PushSubscription? Current { get; set; }
}