﻿using Application.DTOs.Request.Entities;
using Domain.Models.ChatModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Thread = Domain.Models.ChatModels.Thread;

namespace Application.Interfaces
{
    public interface IChatRepository
    {
        Task<List<AvailableUserDto>> AddAvailableUserAsync(AvailableUser model);
        Task<List<AvailableUserDto>> GetAvailableUsersAsync();
        
        Task<List<IndividualChatDto>> GetIndividualChatsAsync(RequestChatDto model);
        Task<List<AvailableUserDto>> RemoveUserAsync(string userId);
        Task AddIndividualChatAsync(Message model);
        
        //Thread Request
        Task<List<ThreadRequestDto>> GetThreadRequestsAsync(string receiverId);
        Task SendThreadRequestAsync(ThreadRequest model);
        Task AcceptThreadRequestAsync(int threadRequestId);
        Task RejectThreadRequestAsync(int threadRequestId);
        
        //Thread Chat
        Task<GeneralResponse> CreateThreadChatAsync(ThreadDto model);
        Task<List<ThreadDto>> GetThreadChatsAsync(string userId);
    }
}
