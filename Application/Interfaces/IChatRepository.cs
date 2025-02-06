using Application.DTOs.Request.Entities;
using Domain.Models.ChatModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thread = Domain.Models.ChatModels.Thread;

namespace Application.Interfaces
{
    public interface IChatRepository
    {
        Task<List<AvailableUserDto>> AddAvailableUserAsync(AvailableUser model);
        Task<List<AvailableUserDto>> GetAvailableUsersAsync();
        Task<List<ThreadRequestDto>> GetThreadRequestsAsync(string ReceiverId);
        Task<List<IndividualChatDto>> GetIndividualChatsAsync(RequestChatDto model);
        Task<List<AvailableUserDto>> RemoveUserAsync(string userId);
        Task AddIndividualChatAsync(Thread model);
        Task SendThreadRequestAsync(ThreadRequest model);
        Task AcceptThreadRequestAsync(int threadRequestId);
        Task RejectThreadRequestAsync(int threadRequestId);
    }
}
