using ApiContracts.DTOs;

namespace BlazorApp.Services;

public class HttpPostService :IPostService
{
    private readonly HttpClient _client;

    public HttpPostService(HttpClient client)
    {
        this._client = client;
    }
    public async Task<List<PostDto>?> GetPostsAsync()
    {
        return await _client.GetFromJsonAsync<List<PostDto>>("post") ?? throw new InvalidOperationException("Posts not found");
    }

    public async Task<PostWithCommentsDto?> GetPostAsync(int id)
    {
        var response = await _client.GetAsync($"post/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PostWithCommentsDto>() ?? throw new InvalidOperationException("Post not found");
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        var response = await _client.PostAsJsonAsync("post", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PostDto>() ?? throw new InvalidOperationException("Something went wrong, try again later");
    }

    public async Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        var response = await _client.PutAsJsonAsync("post", request);
        response.EnsureSuccessStatusCode();
        
    }

    public async Task DeletePostAsync(int id)
    {
        var response = new HttpRequestMessage(HttpMethod.Delete, $"post/{id}");
        var responseMessage = await _client.SendAsync(response);
        responseMessage.EnsureSuccessStatusCode();
    }
}