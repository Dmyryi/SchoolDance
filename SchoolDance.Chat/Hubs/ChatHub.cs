using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using ChatApplication.Interfaces;
using SchoolDance.Chat.Models;

namespace SchoolDance.Chat.Hubs;

public class ChatHub : Hub<IChatClient>
{
    private readonly IDistributedCache _cache;

    public ChatHub(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task JoinChat(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

        var stringConnection = JsonSerializer.Serialize(connection);
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

        await Clients.Group(connection.ChatRoom).ReceiveMessage("Admin", $"{connection.UserName} доєднався до чату");
    }

    public async Task SendMessage(string message)
    {
        var stringConnection = await _cache.GetStringAsync(Context.ConnectionId);
        var connection = stringConnection is not null ? JsonSerializer.Deserialize<UserConnection>(stringConnection) : null;

        if (connection is not null)
            await Clients.Group(connection.ChatRoom).ReceiveMessage(connection.UserName, message);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringConnection = await _cache.GetStringAsync(Context.ConnectionId);
        var connection = stringConnection is not null ? JsonSerializer.Deserialize<UserConnection>(stringConnection) : null;

        if (connection is not null)
        {
            await _cache.RemoveAsync(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);
            await Clients.Group(connection.ChatRoom).ReceiveMessage("Admin", $"{connection.UserName} вийшов з чату");
        }

        await base.OnDisconnectedAsync(exception);
    }
}
