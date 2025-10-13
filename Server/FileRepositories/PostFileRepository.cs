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
        var posts = await LoadAsync();
        int index = posts.FindIndex(p => p.Id == post.Id);
        if (index != -1)
        {
            posts[index] = post;
            await SaveAsync(posts);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var posts = await LoadAsync();
        int removedCount = posts.RemoveAll(p => p.Id == id); 
        if (removedCount > 0)
            await SaveAsync(posts);
    }



    public async Task<Post?> GetSingleAsync(int id)
    {
        var posts = await LoadAsync();
        return posts.FirstOrDefault(p => p.Id == id);
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