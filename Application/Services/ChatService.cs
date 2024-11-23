using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Models.ChatModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ChatService : IChatRepository
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<AvailableUserDto>> AddAvailableUserAsync(AvailableUser model)
        {
            throw new NotImplementedException();
        }

        public Task<GroupChatDto> AddChatToGroupAsync(GroupChat model)
        {
            throw new NotImplementedException();
        }

        public Task AddIndividualChatAsync(IndividualChat model)
        {
            throw new NotImplementedException();
        }

        public Task<List<AvailableUserDto>> GetAvailableUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<GroupChatDto>> GetGroupChatsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<IndividualChatDto>> GetIndividualChatsAsync(RequestChatDto model)
        {
            throw new NotImplementedException();
        }

         public async Task<List<AvailableUserDto>> RemoveUserAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/chats/remove-user/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to remove user");
            }

            return new List<AvailableUserDto>();
        }
    }
}
