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
        string name = "Default";
        string password = "Default";
        Console.WriteLine("Name: ");
        string? temp = Console.ReadLine();
        if (temp is not null)
        {
            name = temp;
        }

        Console.WriteLine("Password: ");
        temp = temp = Console.ReadLine();
        if (temp is not null)
        {
            password = temp;
        }

        if (password.Equals("Default") || name.Equals("Default"))
        {
            Console.WriteLine("Something went wrong, try again");
            await Task.CompletedTask;
        }
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