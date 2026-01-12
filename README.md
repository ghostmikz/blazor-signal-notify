# 🚀 Blazor SignalNotify

A lightweight .NET demo showing how to send SignalR Live Updates and Web Push Notifications between different user roles (Employee & Manager).

## ⚡ Quick Start

1. **Run the Server:**
   `cd Server && dotnet run`

2. **Run the Client:**
   `cd Client && dotnet run`

3. **Test the Flow:**
   - **Manager:** Login as `manager`. Enable notifications. You will see a persistent history table.
   - **Employee:** Login as `worker` in a separate/incognito window.
   - **Action:** Click "Send Request." The Employee gets a UI Toast, and the Manager receives a Desktop Push Notification.

## ✨ Key Features
- **Role-Based Push:** Notifications are routed specifically to the "Manager" role.
- **Persistent Inbox:** Requests are logged on the server; Managers see missed requests upon logging back in.
- **Clean Logout:** Automatically unsubscribes the browser and server-side store to prevent orphaned notifications.

## 🛠️ Tech Stack
- **Blazor WebAssembly** (Frontend)
- **ASP.NET Core Web API** (Backend)
- **Lib.Net.Http.WebPush** (VAPID Notifications)
- **SignalR** (Real-time UI sync)