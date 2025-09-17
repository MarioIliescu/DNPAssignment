using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;
public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Name: ");
        string? name = Console.ReadLine();
        Console.WriteLine("Password: ");
        string? password = Console.ReadLine();
        await AddUserAsync(name, password);
        await Task.CompletedTask;
    }
    public async Task<User> AddUserAsync(string name, string password)
    {
        User user = new()
        {
            Username = name,
            Password = password
        };

        User created = await userRepository.AddAsync(user);
        Console.WriteLine(created.ToString());
        return created;
    }
}