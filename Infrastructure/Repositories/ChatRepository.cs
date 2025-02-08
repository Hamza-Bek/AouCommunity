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
using Application.DTOs.Response;
using AutoMapper;
using Thread = Domain.Models.ChatModels.Thread;

namespace Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public ChatRepository(AppDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
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

        public async Task AddIndividualChatAsync(Message model)
        {
            model.Date = DateTime.UtcNow;
            _context.Messages.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<IndividualChatDto>> GetIndividualChatsAsync(RequestChatDto model)
        {
            var ChatList = new List<IndividualChatDto>();
            var chats = await _context.Messages
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
                        Message = chat.Content,
                        Date = chat.Date
                    });
                }
                return ChatList;
            }
            return null;
        }


        #region Thread Request
        public async Task<List<ThreadRequestDto>> GetThreadRequestsAsync(string receiverId)
        {
            var list = new List<ThreadRequestDto>();
            var threadRequest = await _context.ThreadRequests
                .Where(r => r.ReceiverId == receiverId)
                .Where(s => s.Status == ConnectionRequestStatus.Pending)
                .ToListAsync();

            foreach (var u in threadRequest)
            {
                var userIdentity = await _userManager.FindByIdAsync(u.SenderId);
                list.Add(new ThreadRequestDto()
                {
                    Id = u.Id,
                    SenderId = u.SenderId,
                    ReceiverId = u.ReceiverId,
                    Status = ConnectionRequestStatus.Pending,
                    SentDate = DateTime.UtcNow,
                    SenderFullname = userIdentity?.UserName
                });
            }
            return list;
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

        public async Task AcceptThreadRequestAsync(int threadRequestId)
        {
            var threadRequest = await _context.ThreadRequests.FindAsync(threadRequestId);
            if (threadRequest is not null)
            {
                try
                {
                    threadRequest.Status = ConnectionRequestStatus.Accepted;

                    var newThread = new ThreadDto()
                    {
                        ThreadId = Guid.NewGuid().ToString(),
                        User1Id = threadRequest.SenderId,
                        User2Id = threadRequest.ReceiverId
                    };

                    await CreateThreadChatAsync(newThread);
                    
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in AcceptThreadRequestAsync: {ex.Message}");
                    throw new Exception(ex.Message);

                }
            }
        }

        public async Task RejectThreadRequestAsync(int threadRequestId)
        {
            var threadRequest = await _context.ThreadRequests.FindAsync(threadRequestId);
            if (threadRequest is not null)
            {
                threadRequest.Status = ConnectionRequestStatus.Rejected;
                await _context.SaveChangesAsync();
            }   
        }

        #endregion

        #region Thread Chat
        
        public async Task<GeneralResponse> CreateThreadChatAsync(ThreadDto model)
        {
            var map = _mapper.Map<ThreadDto>(model);
            
            var user1 = await _context.Users.Where(p => p.Id == model.User1Id).FirstOrDefaultAsync();
            var user2 = await _context.Users.Where(p => p.Id == model.User2Id).FirstOrDefaultAsync();
            if(user1 is null || user2 is null)
            {
                throw new Exception("User not found.");
            }
            
            var user1ConnectionId = await _context.AvailableUsers.Where(p => p.UserId == model.User1Id).FirstOrDefaultAsync();
            var user2ConnectionId = await _context.AvailableUsers.Where(p => p.UserId == model.User2Id).FirstOrDefaultAsync();
            if(user1ConnectionId is null || user2ConnectionId is null)
            {
                throw new Exception("Connection ID not found.");
            }
            
            var newThread = new Thread()
            {                
                ThreadId = Guid.NewGuid().ToString(),
                User1Id = model.User1Id,
                User2Id = model.User2Id,
                User1ConnectionId = user1ConnectionId.ConnectionId,
                User2ConnectionId = user2ConnectionId.ConnectionId
            };
            
            await _context.Threads.AddAsync(newThread);
            await _context.SaveChangesAsync();
            
            return new GeneralResponse(true,"Thread created successfully.");
        }

        public async Task<List<ThreadDto>> GetThreadChatsAsync(string userId)
        {
            var list = new List<ThreadDto>();
            var threads = await _context.Threads
                .Where(p => p.User1Id == userId || p.User2Id == userId)
                .ToListAsync();

            foreach (var t in threads)
            {
                list.Add(new ThreadDto()
                {
                    ThreadId = t.ThreadId,
                    User1Id = t.User1Id,
                    User2Id = t.User2Id,
                    User1ConnectionId = t.User1ConnectionId,
                    User2ConnectionId = t.User2ConnectionId
                });
            }

            return list;
        }

        #endregion
        
    }
}
