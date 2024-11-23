using Application.DTOs.Request.Entities;
using Application.DTOs.Response;
using Domain.Models;

namespace Application.Interfaces;

public interface IPostRepository
{
    Task<GeneralResponse> CreatePostAsync(Post post, string userId);
    Task<GeneralResponse> RemovePostAsync(string postId , string userId);
    Task<Post> GetPostByIdAsync(string postId);
    Task<IEnumerable<Post>> GetAllPostsAsync();
    
    //
    Task<IEnumerable<Post>> GetPendingPostsAsync();
    Task<GeneralResponse> ApprovePostAsync(string postId);
    Task<GeneralResponse> RejectPostAsync(string postId);
    
    //COMMENT SECTION
    Task<GeneralResponse> CreateCommentAsync(Comment model, string userId);
    Task<GeneralResponse> DeleteCommentAsync(string ComemntId, string UserId);
    Task<Comment> GetCommentByIdAsync(string CommentId);
    Task<IEnumerable<Comment>> GetAllCommentsAsync();

}