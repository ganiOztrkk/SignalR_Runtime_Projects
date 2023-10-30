using Microsoft.AspNetCore.SignalR;

namespace SignalRServerExample.Hubs;

public class MyHub : Hub // tcp protokolü üzerinden haberleşmemizi sağlayan operasyonu üstlenen sınıf. bu protokolde 1 server 1 hub ve 1den fazla client olabilir. clientlarda ilgili fonksiyonların tetiklenmesi işlemi RPC teknolojisi sayesinde olur.
{
    private static readonly List<string> clients = new List<string>();
    
    /*public async Task SendMessageAsync(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }*/

    public override async Task OnConnectedAsync()
    {
        clients.Add(Context.ConnectionId);
        await Clients.All.SendAsync("Clients", clients);
        await Clients.All.SendAsync("UserJoined", Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        clients.Remove(Context.ConnectionId);
        await Clients.All.SendAsync("Clients", clients);
        await Clients.All.SendAsync("UserLeaved", Context.ConnectionId);
    }
}