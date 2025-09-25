using System.Text.Json;
using Entities;
using RepositoryContracts;
namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await LoadAsync();
        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 1;
        user.Id = maxId + 1;
        users.Add(user);
        await SaveAsync(users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = await LoadAsync();
        users[users.IndexOf(user)] = user;
        await SaveAsync(users);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        List<User> users = await LoadAsync();
        users.Remove(users[id]);
        await SaveAsync(users);
        await Task.CompletedTask;
    }

    public async Task<User> GetSingleAsync(int id)
    {
        List<User> users = await LoadAsync();
        User user = users[id];
        return await Task.FromResult(user);
    }

    public IQueryable<User> GetManyAsync()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }

    private async Task SaveAsync(List<User> users)
    {
        string usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        await Task.CompletedTask;
    }

    private async Task<List<User>> LoadAsync()
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return await Task.FromResult(users);
    }
}