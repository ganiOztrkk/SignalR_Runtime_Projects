using Microsoft.AspNetCore.SignalR;

namespace SignalRServerExample.Hubs;

public class MyHub : Hub // tcp protokolü üzerinden haberleşmemizi sağlayan operasyonu üstlenen sınıf. bu protokolde 1 server 1 hub ve 1den fazla client olabilir. clientlarda ilgili fonksiyonların tetiklenmesi işlemi RPC teknolojisi sayesinde olur.
{
    public async Task SendMessageAsync(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public override Task OnConnectedAsync()
    {
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        
        return base.OnDisconnectedAsync(exception);
    }
}