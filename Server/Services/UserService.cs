namespace Server.Services;

public class UserProfile
{
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Manager" or "Employee"
}

public static class UserService
{
    // Mock Database
    private static readonly List<UserProfile> _users = new()
    {
        new UserProfile { Username = "Manager", Role = "Manager" },
        new UserProfile { Username = "worker1", Role = "Employee" }
    };

    public static UserProfile? Authenticate(string username, string password)
    {
        // For testing, password is just 'password'
        if (password != "password") return null;
        return _users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
    }
}