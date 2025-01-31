using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using Domain.Models.ChatModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thread = Domain.Models.ChatModels.Thread;

namespace Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }        

        public async Task<List<AvailableUserDto>> AddAvailableUserAsync(AvailableUser model)
        {
            var List = new List<AvailableUserDto>();

            var getUser = await _context.AvailableUsers
                .FirstOrDefaultAsync(u => u.UserId == model.UserId);
            if (getUser is not null)
                getUser.ConnectionId = model.ConnectionId;
            else
                _context.AvailableUsers.Add(model);

            await _context.SaveChangesAsync();

            var allUsers = await _context.AvailableUsers.ToListAsync();

            foreach(var user in allUsers)
            {
                List.Add(new AvailableUserDto()
                {
                    UserId = user.UserId,                    
                    Fullname = (await _userManager.FindByIdAsync(user.UserId!)).UserName
                });
            }

            return List;
        }

        public async Task<List<AvailableUserDto>> GetAvailableUsersAsync()
        {
            var list = new List<AvailableUserDto>();
            var users = await _context.AvailableUsers.ToListAsync();

            foreach (var u in users)
            {
                var userIdentity = await _userManager.FindByIdAsync(u.UserId!);

                if (userIdentity != null)
                {
                    list.Add(new AvailableUserDto()
                    {
                        UserId = u.UserId,
                        Fullname = userIdentity.UserName
                    });
                }
                else
                {
                    Console.WriteLine($"User with ID {u.UserId} not found in UserManager.");
                }
            }

            return list;
        }

        public async Task<List<AvailableUserDto>> RemoveUserAsync(string userId)
        {
            var user = await _context.AvailableUsers.FirstOrDefaultAsync(u => u.UserId == userId);
            
            if(user is not null)
            {
                _context.AvailableUsers.Remove(user);
                await _context.SaveChangesAsync();
            }

            var List = new List<AvailableUserDto>();
            var users = await _context.AvailableUsers.ToListAsync();
            foreach(var u in users)
            {
                List.Add(new AvailableUserDto()
                {
                    UserId = u.UserId,
                    Fullname = (await _userManager.FindByIdAsync(user.UserId!)).UserName,
                });
            }

            return List;
        }

        public async Task AddIndividualChatAsync(Thread model)
        {
            model.Date = DateTime.UtcNow;
            _context.Threads.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<IndividualChatDto>> GetIndividualChatsAsync(RequestChatDto model)
        {
            var ChatList = new List<IndividualChatDto>();
            var chats = await _context.Threads
                .Where(s => s.SenderId == model.SenderId && s.ReceiverId == model.ReceiverId || s.SenderId == model.ReceiverId && s.ReceiverId == model.SenderId)
                .ToListAsync();

            if(chats is not null)
            {
                foreach(var chat in chats)
                {
                    ChatList.Add(new IndividualChatDto()
                    {
                        SenderId = chat.SenderId,
                        ReceiverId = chat.ReceiverId,
                        SenderName = (await _userManager.FindByIdAsync(chat.SenderId!)).UserName,
                        ReceiverName = (await _userManager.FindByIdAsync(chat.ReceiverId!)).UserName,
                        Message = chat.Message,
                        Date = chat.Date
                    });
                }
                return ChatList;
            }
            return null;
        }

        public async Task SendThreadRequestAsync(ThreadRequest model)
        {
            var sender = await _userManager.FindByIdAsync(model.SenderId);
            if(sender is null)
            {
                throw new Exception("Sender not found.");
            }

            var receiver = await _userManager.FindByIdAsync(model.ReceiverId);
            if (receiver is null)
            {
                throw new Exception("Receiver not found.");
            }           
            
            var request = new ThreadRequest()
            {                
                SenderId = model.SenderId,
                ReceiverId = model.ReceiverId,
            };

            await _context.ThreadRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }
    }
}
