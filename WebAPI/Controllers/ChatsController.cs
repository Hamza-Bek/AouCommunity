using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
