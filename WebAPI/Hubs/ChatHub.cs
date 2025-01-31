using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Thread = Domain.Models.ChatModels.Thread;

namespace WebAPI.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatRepository _chatRepository;
    
    public ChatHub(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    private static readonly Dictionary<string, string> UserConnections = new Dictionary<string, string>();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
    
        if (!string.IsNullOrEmpty(userId))
        {
            // Store the user's connection ID
            UserConnections[userId] = Context.ConnectionId;

            // Send a message to the connected user
            await Clients.Caller.SendAsync("ReceiveMessage", $"Hello {userId}, you are now connected!");

            Console.WriteLine($"User {userId} connected with connection ID: {Context.ConnectionId}");
        }

        await base.OnConnectedAsync();
    }
    
    public async Task AddAvailableUserAsync(AvailableUser model)
    {
        model.ConnectionId = Context.ConnectionId;
        await _chatRepository.AddAvailableUserAsync(model);
        var  availableUsers = await _chatRepository.GetAvailableUsersAsync();
        await Clients.All.SendAsync("NotifyAllClients", availableUsers); 
    }

    public async Task RemoveUserAsync(string userId)
    {
        await _chatRepository.RemoveUserAsync(userId);
        var availableUsers = await _chatRepository.GetAvailableUsersAsync();
        await Clients.All.SendAsync("NotifyAllClients", availableUsers);
    }
    
    public async Task AddIndividualChat(Thread model)
    {
        await _chatRepository.AddIndividualChatAsync(model);

        var requestDto = new RequestChatDto()
        {
            ReceiverId = model.ReceiverId,
            SenderId = model.SenderId
        };

        var getChats = await _chatRepository.GetIndividualChatsAsync(requestDto);

        var prepareIndividualChat = new IndividualChatDto()
        {
            SenderId = model.SenderId,
            ReceiverId = model.ReceiverId,
            SenderName = getChats?.Where(c => c.SenderId == model.SenderId).FirstOrDefault()!.SenderName,
            ReceiverName = getChats?.Where(c => c.ReceiverId == model.ReceiverId).FirstOrDefault()!.ReceiverName,
            Message = model.Message,
            Date = model.Date
        };
        
        //THE SERVER OR CLIENT SIDE CAN NOT GET OR DEFINE THE USER ID (ReceiverId)
        await Clients.User(model.ReceiverId!).SendAsync("ReceiveIndividualMessage", prepareIndividualChat);
        
    }

    public async Task<List<AvailableUserDto>> GetUsers() => await _chatRepository.GetAvailableUsersAsync();

    public async Task SendThreadRequest(ThreadRequest model)
    {
        await _chatRepository.SendThreadRequestAsync(model);
        await Clients.User(model.ReceiverId!).SendAsync("ReceiveThreadRequest", model);
    }
}