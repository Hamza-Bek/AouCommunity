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
using Thread = Domain.Models.ChatModels.Thread;

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

        public Task AddIndividualChatAsync(Thread model)
        {
            throw new NotImplementedException();
        }

        public Task<List<AvailableUserDto>> GetAvailableUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ThreadRequestDto>> GetThreadRequestsAsync(string ReceiverId)
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

        public Task SendThreadRequestAsync(ThreadRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
