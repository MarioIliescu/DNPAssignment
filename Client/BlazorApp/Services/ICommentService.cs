using ApiContracts.DTOs;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<List<CommentDto>?> GetCommentsAsync();
    public Task<List<CommentDto>> GetCommentFromPostAsync(int id);
    public Task<CommentDto?> GetCommentAsync(int id);
    public Task<CommentDto> AddCommentAsync(CreateCommentDto request); 
    public Task UpdateCommentAsync(int id, UpdateCommentDto request); 
    public Task DeleteCommentAsync(int id);
}