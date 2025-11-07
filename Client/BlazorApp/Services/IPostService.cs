using ApiContracts.DTOs;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<List<PostDto>?> GetPostsAsync();
    public Task<PostWithCommentsDto?> GetPostAsync(int id);
    public Task<PostDto> AddPostAsync(CreatePostDto request); 
    public Task UpdatePostAsync(int id, UpdatePostDto request); 
    public Task DeletePostAsync(int id);
}