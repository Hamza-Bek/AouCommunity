using Application.DTOs.Request.Entities;
using Domain.Models.ChatModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IChatRepository
    {
        Task<List<AvailableUserDto>> AddAvailableUserAsync(AvailableUser model);
        Task<GroupChatDto> AddChatToGroupAsync(GroupChat model);
        Task AddIndividualChatAsync(IndividualChat model);

        Task<List<GroupChatDto>> GetGroupChatsAsync();
        Task<List<AvailableUserDto>> GetAvailableUsersAsync();
        Task<List<IndividualChatDto>> GetIndividualChatsAsync(RequestChatDto model);
        
        Task<List<AvailableUserDto>> RemoveUserAsync(string userId);
    }
}
