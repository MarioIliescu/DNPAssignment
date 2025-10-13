using System.Text.Json;
using Entities;
using RepositoryContracts;
namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        List<Comment> comments = await LoadAsync();
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 1;
        comment.Id = maxId + 1;
        comments.Add(comment);
        await SaveAsync(comments);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var comments = await LoadAsync();
        int index = comments.FindIndex(p => p.Id == comment.Id);
        if (index != -1)
        {
            comments[index] = comment;
            await SaveAsync(comments);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var comments = await LoadAsync();
        int removedCount = comments.RemoveAll(p => p.Id == id); 
        if (removedCount > 0)
            await SaveAsync(comments);
    }


    public async Task<Comment?> GetSingleAsync(int id)
    {
        var posts = await LoadAsync();
        return posts.FirstOrDefault(p => p.Id == id);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }

    private async Task SaveAsync(List<Comment> comments)
    {
        string commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        await Task.CompletedTask;
    }

    private async Task<List<Comment>> LoadAsync()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return await Task.FromResult(comments);
    }
}