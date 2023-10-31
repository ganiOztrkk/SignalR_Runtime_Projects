using Microsoft.AspNetCore.SignalR;
using SignalRChatServer.Data;
using SignalRChatServer.Models;

namespace SignalRChatServer.Hubs;

public class ChatHub : Hub
{
    public async Task GetNickName(string nickName)
    {
        var client = new Client()
        {
            ConnectionId = Context.ConnectionId,
            NickName = nickName
        };
        
        ClientSource.Clients.Add(client);
        await Clients.Others.SendAsync("clientJoined", nickName);
        await Clients.All.SendAsync("clients", ClientSource.Clients);
    }

    public async Task SendMessageAsync(string message, string clientName)
    {
        clientName = clientName.Trim().ToLower();
        Client senderClient = ClientSource.Clients.FirstOrDefault(_ => _.ConnectionId == Context.ConnectionId);
        if (clientName == "tümü")
        {
             await Clients.Others.SendAsync("receiveMessage", message, senderClient.NickName);
        }
        else
        {
            Client client = ClientSource.Clients.FirstOrDefault(_ => _.NickName == clientName);
            await Clients.Client(client!.ConnectionId).SendAsync("receiveMessage", message, senderClient.NickName);
        }
    }

    public async Task AddGroup(string groupName)
    {
        Group group = new Group { GroupName = groupName };
        group.Clients.Add(ClientSource.Clients.FirstOrDefault(_ => _.ConnectionId == Context.ConnectionId));
        
        GroupSource.Groups.Add(group);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.All.SendAsync("groups", GroupSource.Groups);
    }

    public async Task AddClientToGroup(IEnumerable<string> groupNames)
    {
        Client client = ClientSource.Clients.FirstOrDefault(_ => _.ConnectionId == Context.ConnectionId);
        foreach (var item in groupNames)
        {
            Group _group = GroupSource.Groups.FirstOrDefault(x => x.GroupName == item);

            var result = _group.Clients.Any(x => x.ConnectionId == Context.ConnectionId);
            if (!result)
            {
                _group!.Clients.Add(client!);
                await Groups.AddToGroupAsync(Context.ConnectionId, item);    
            }
        }
    }

    public async Task GetClientToGroup(string groupName)
    {
        if (groupName is "-1")
            await Clients.Caller.SendAsync("clients", ClientSource.Clients);
        
        Group group = GroupSource.Groups.FirstOrDefault(x => x.GroupName == groupName);

        await Clients.Caller.SendAsync("clients", group.Clients);
    }

    public async Task SendMessageToGroupAsync(string groupName, string message)
    {
        await Clients.Group(groupName).SendAsync("receiveMessage", message, ClientSource.Clients.FirstOrDefault(_ => _.ConnectionId == Context.ConnectionId).NickName);
    }
}