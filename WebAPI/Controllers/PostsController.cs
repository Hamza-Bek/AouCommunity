using System.Runtime.InteropServices.JavaScript;
using Application.DTOs.Request.Entities;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using Infrastructure.Data;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IValidator<PostDto> _postValidator;

    public PostsController(IValidator<PostDto> postValidator, IPostRepository postRepository)
    {
        _postValidator = postValidator;
        _postRepository = postRepository;
    }

    [HttpPost("create/post")]
    public async Task<IActionResult> CreatePostAsync([FromBody] PostDto model, string userId)
    {
        var validationResult = await _postValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var post = new Post()
        {
            Id = Guid.NewGuid().ToString(),
            Content = model.Content,
            CreatedDate = DateTime.UtcNow,
            UserId = userId,
            Status = EntityStatus.PendingApproval
        };

        var result = await _postRepository.CreatePostAsync(post , userId);
        return Ok(result);
    }

    [HttpDelete("remove/post")]
    public async Task<IActionResult> RemovePostAsync(string postId, string userId)
    {
        var result = await _postRepository.RemovePostAsync(postId, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("get/post")]
    public async Task<IActionResult> FindPostByIdAsync(string postId)
    {
        var result = await _postRepository.GetPostByIdAsync(postId);
        return Ok(result);
    }

    [HttpGet("get/posts")]
    public async Task<IActionResult> GetAllPostsAsync()
    {
        var result = await _postRepository.GetAllPostsAsync();
        return Ok(result);
    }
    
    
    [HttpGet("get/pending-posts")]
    public async Task<IActionResult> GetPendingPostsAsync()
    {
        var result = await _postRepository.GetPendingPostsAsync();
        return Ok(result);
    }

    [HttpPost("approve/post")]
    public async Task<IActionResult> ApprovePostAsync(string postId)
    {
        var result = await _postRepository.ApprovePostAsync(postId);
        return Ok(result);
    }

    [HttpPost("reject/post")]
    public async Task<IActionResult> RejectPostAsync(string postId)
    {
        var result = await _postRepository.RejectPostAsync(postId);
        return Ok(result);
    }
}