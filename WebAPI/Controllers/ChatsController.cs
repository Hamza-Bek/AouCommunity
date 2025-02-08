using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thread = Domain.Models.ChatModels.Thread;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : Controller
    {
        private readonly IChatRepository _chatRepository;

        public ChatsController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpDelete("remove-user/{userId}")]
        public async Task<ActionResult> RemoveUserAsync(string userId)
        {
            await _chatRepository.RemoveUserAsync(userId);
            return NoContent();
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync() => Ok(await _chatRepository.GetAvailableUsersAsync());

        [HttpPost("individual")]
        public async Task<IActionResult> GetIndividualChatsAsync(RequestChatDto model) =>
            Ok(await _chatRepository.GetIndividualChatsAsync(model));      
        
        [HttpPost("thread/request")]
        public async Task<IActionResult> RequestThreadAsync(ThreadRequest model)
        {
            await _chatRepository.SendThreadRequestAsync(model);
            return NoContent();
        }        
        
        [HttpGet("thread/receive/")]
        public async Task<IActionResult> ReceiveThreadRequestAsync([FromQuery]string ReceiverId)
        {
            var result = await _chatRepository.GetThreadRequestsAsync(ReceiverId);
            return Ok(result);
        }
        
        [HttpPost("thread/accept")]
        public async Task<IActionResult> AcceptThreadRequestAsync([FromQuery]int threadRequestId)
        {
            await _chatRepository.AcceptThreadRequestAsync(threadRequestId);
            return NoContent();
        }
        
        [HttpPost("thread/reject")]
        public async Task<IActionResult> RejectThreadRequestAsync([FromQuery]int threadRequestId)
        {
            await _chatRepository.RejectThreadRequestAsync(threadRequestId);
            return NoContent();
        }
        [HttpPost("thread/add/thread")]
        public async Task<IActionResult> AddThreadChatAsync(ThreadDto model)
        {
            try
            {
                var response = await _chatRepository.CreateThreadChatAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            
        [HttpGet("thread/get/threads")]
        public async Task<IActionResult> GetThreadChatsAsync([FromQuery]string userId)
        {
            var response = await _chatRepository.GetThreadChatsAsync(userId);
            return Ok(response);
        }
       
    }
}
