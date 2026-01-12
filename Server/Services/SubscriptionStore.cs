using System.Collections.Concurrent;
using Lib.Net.Http.WebPush;

namespace Server.Services;

public static class SubscriptionStore
{
    private static readonly ConcurrentDictionary<string, PushSubscription> _roleSubscriptions = new();
    
    // This acts as the "Database" for the table
    private static readonly List<RequestLog> _allRequests = new();

    public static void AddSubscription(string role, PushSubscription sub) => _roleSubscriptions[role] = sub;
    public static bool TryGetSubscription(string role, out PushSubscription sub) => _roleSubscriptions.TryGetValue(role, out sub!);
    public static void RemoveSubscription(string role) => _roleSubscriptions.TryRemove(role, out _);

    // Methods for the Table
    public static void LogRequest(string message) 
    {
        lock(_allRequests) {
            _allRequests.Add(new RequestLog { Message = message, Time = DateTime.Now });
        }
    }

    public static List<RequestLog> GetLogs() 
    {
        lock(_allRequests) {
            return _allRequests.OrderByDescending(x => x.Time).ToList();
        }
    }
}

public class RequestLog {
    public string Message { get; set; } = "";
    public DateTime Time { get; set; }
}