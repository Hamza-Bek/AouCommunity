using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

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
    
    // public async Task GetThreadsAsync(string userId)
    // {
    //     var result = await _chatRepository.GetThreadsAsync(userId);
    //     await Clients.User(userId).SendAsync("ReceiveThreads", result);
    // }

    public async Task RemoveUserAsync(string userId)
    {
        await _chatRepository.RemoveUserAsync(userId);
        var availableUsers = await _chatRepository.GetAvailableUsersAsync();
        await Clients.All.SendAsync("NotifyAllClients", availableUsers);
    }
    
    public async Task AddIndividualChat(Message model)
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
            Message = model.Content,
            Date = model.Date
        };
        
        await Clients.User(model.ReceiverId!).SendAsync("ReceiveIndividualMessage", prepareIndividualChat);
        
    }

    public async Task<List<AvailableUserDto>> GetUsers() => await _chatRepository.GetAvailableUsersAsync();


    #region Thread Request

    public async Task SendThreadRequest(ThreadRequest model)
    {
        await _chatRepository.SendThreadRequestAsync(model);
        await Clients.User(model.ReceiverId!).SendAsync("ReceiveThreadRequest", model);
    }
    
    public async Task ReceiveThreadRequest(string ReceiverId)
    {
        var result = await _chatRepository.GetThreadRequestsAsync(ReceiverId);
        await Clients.User(ReceiverId).SendAsync("ReceiveThreadRequest", result);
    }
    
    public async Task AcceptThreadRequest(ThreadRequest model)
    {
        await _chatRepository.AcceptThreadRequestAsync(model.Id);
        await Clients.User(model.SenderId!).SendAsync("AcceptThreadRequest", model);
    }

    public async Task RejectThreadRequest(ThreadRequest model)
    {
        await _chatRepository.RejectThreadRequestAsync(model.Id);
        await Clients.User(model.SenderId!).SendAsync("RejectThreadRequest", model);
    }

    #endregion
  
}