using ApiContracts.DTOs;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient _client;

    public HttpCommentService(HttpClient client)
    {
        this._client = client;
    }
    public async Task<List<CommentDto>?> GetCommentsAsync()
    {
        return await _client.GetFromJsonAsync<List<CommentDto>>("comments") ?? throw new InvalidOperationException("Comments not found");
    }

    public async Task<List<CommentDto>> GetCommentFromPostAsync(int id)
    {
        return await _client.GetFromJsonAsync<List<CommentDto>>($"comments/post/{id}") ?? throw new InvalidOperationException("Comments not found");
    }

    public async Task<CommentDto?> GetCommentAsync(int id)
    {
       var response = await _client.GetAsync($"comments/{id}");
       response.EnsureSuccessStatusCode();
       return await response.Content.ReadFromJsonAsync<CommentDto>() ?? throw new InvalidOperationException("Comment not found");
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        var response = await _client.PostAsJsonAsync("comments", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CommentDto>() ?? throw new InvalidOperationException("Something went wrong, try again later");
    }

    public async Task UpdateCommentAsync(int id, UpdateCommentDto request)
    {
        var response = await _client.PutAsJsonAsync("comments", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCommentAsync(int id)
    {
        var response = new HttpRequestMessage(HttpMethod.Delete, $"comments/{id}");
        var responseMessage = await _client.SendAsync(response);
        responseMessage.EnsureSuccessStatusCode();
    }
}