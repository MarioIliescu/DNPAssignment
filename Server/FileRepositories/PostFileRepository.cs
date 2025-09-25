using System.Text.Json;
using Entities;
using RepositoryContracts;
namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadAsync();
        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) : 1;
        post.Id = maxId + 1;
        posts.Add(post);
        await SaveAsync(posts);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadAsync();
        posts[posts.IndexOf(post)] = post;
        await SaveAsync(posts);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await LoadAsync();
        posts.Remove(posts[id]);
        await SaveAsync(posts);
        await Task.CompletedTask;
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await LoadAsync();
        Post post = posts[id];
        return await Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }

    private async Task SaveAsync(List<Post> posts)
    {
        string postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        await Task.CompletedTask;
    }

    private async Task<List<Post>> LoadAsync()
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return await Task.FromResult(posts);
    }
}