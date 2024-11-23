using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralResponse> CreatePostAsync(Post post, string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if(user is null)
                return new GeneralResponse(false , "User does not exist");

            _context.Posts.Add(post);
            
            await _context.SaveChangesAsync();
            
            return new GeneralResponse(true , "Post Submitted, Please wait for the admin approval");
        }
        catch(Exception ex)
        {
            return new GeneralResponse(false , ex.Message);
        }
    }

    public async Task<GeneralResponse> RemovePostAsync(string postId, string userId)
    {
        try
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post is null)
                return new GeneralResponse(false, "Post does not exist");
            
            if(post.UserId != userId)
                return new GeneralResponse(false, "User does not belong to this post");
                
            _context.Posts.Remove(post);

            await _context.SaveChangesAsync();
            
            return new GeneralResponse(true , "Post removed successfully");
        }
        catch(Exception ex)
        {
            return new GeneralResponse(false, ex.Message);
        }
    }

    public async Task<Post> GetPostByIdAsync(string postId)
    {
        try
        {
            var post = await _context.Posts
                .Where(i => i.Id == postId)
                .Where(s => s.Status == EntityStatus.Approved) 
                .Include(p => p.User)
                .FirstOrDefaultAsync();
            
            return post!;
        }
        catch
        {
            return null!;
        }
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        try
        {
            var posts = await _context.Posts.Where(s => s.Status == EntityStatus.Approved).ToListAsync();
            return posts;
        }
        catch
        {
            return null!;
        }
    }

    public async Task<IEnumerable<Post>> GetPendingPostsAsync()
    {
        return await _context.Posts.Where(s => s.Status == EntityStatus.PendingApproval).ToListAsync();
    }

    public async Task<GeneralResponse> ApprovePostAsync(string postId)
    {
        var post = await _context.Offers.FindAsync(postId);
        if(post is null)
            return new GeneralResponse(false , "No post found to proceed this action");

        post.Status = EntityStatus.Approved;

        _context.Offers.Update(post);
        await _context.SaveChangesAsync();

        return new GeneralResponse(true, "Post approved!");
    }

    public async Task<GeneralResponse> RejectPostAsync(string postId)
    {
        var post = await _context.Offers.FindAsync(postId);
        if(post is null)
            return new GeneralResponse(false , "No post found to proceed this action");

        post.Status = EntityStatus.Rejected;

        _context.Offers.Update(post);
        await _context.SaveChangesAsync();

        return new GeneralResponse(true, "Post rejected!");
    }

    public Task<GeneralResponse> CreateCommentAsync(Comment model, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResponse> DeleteCommentAsync(string ComemntId, string UserId)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> GetCommentByIdAsync(string CommentId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Comment>> GetAllCommentsAsync()
    {
        throw new NotImplementedException();
    }
}